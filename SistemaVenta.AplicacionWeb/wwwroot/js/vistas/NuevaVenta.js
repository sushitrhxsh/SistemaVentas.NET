
let ValorImpuesto = 0;

$(document).ready(function (){

    // # Llenear el combobox de #cboTipoDocumentoVenta
    fetch("/Venta/ListaTipoDocumentoVenta") 
    .then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {
        if(responseJson.length > 0){
            responseJson.forEach((item) => {
                $("#cboTipoDocumentoVenta").append(
                    $("<option>").val(item.idTipoDocumentoVenta).text(item.descripcion)
                );
            });
        }
    });

    // Llenar el detalle de los spans de los inputs
    fetch("/Negocio/Obtener") 
    .then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {
        if(responseJson.estado){
            const d = responseJson.objeto;
            //console.log(d);

            $("#inputGroupSubTotal").text(`Sub total - ${d.simboloMoneda}`);
            $("#inputGroupIGV").text(`IGV(${d.porcentajeImpuesto}%) - ${d.simboloMoneda}`);
            $("#inputGroupTotal").text(`Total - ${d.simboloMoneda}`);
            ValorImpuesto = parseFloat(d.porcentajeImpuesto);
        }
    });

    // Select2 
    $("#cboBuscarProducto").select2({
        ajax: {
          url: "/Venta/ObtenerProductos",
          dataType: 'json',
          contentType:"application/json;charset=utf-8",
          delay: 250,
          data: function (params) {
            return {
              busqueda: params.term
            };
          },
          processResults: function (data) {
            return {
              results: data.map((item)=> (
                {
                    id:item.idProducto,
                    text:item.descripcion,
                    marca:item.marca,
                    categoria:item.nombreCategoria,
                    urlImagen:item.urlImagen,
                    precio:parseFloat(item.precio)
                }
              ))
            };
          },
        },
        language:"es",
        placeholder: 'Buscar Producto...',
        minimumInputLength: 1,
        templateResult: formatResultados,
    });
});

/* ----------------------------------------- *
 *    Funciones modals, selects y renders    *
 * ----------------------------------------- */
// Mostrar resultados en tabla al select2
function formatResultados(data){
    if(data.loading){
        return data.text;
    }

    var container = $(`
        <table width="100%">
            <tr>
                <td>
                    <img style="height:60px;width:60px;margin-right:10px" src="${data.urlImagen}" />
                </td>
                <td>
                    <p style="font-weight:bolder;margin:2px">${data.marca}</p>
                    <p style="margin:2px">${data.text}</p>
                </td>             
            </tr>
        </table>
    
    `);

    return container;
}


/* ------------------------------------- *
 *       Funciones de acciones           *
 * ------------------------------------- */
// Seleccionar cursor de forma automatica select2
$(document).on("select2:open",function (){
    document.querySelector(".select2-search__field").focus();
});

// Mostrar informacion despues de buscarlo y seleccionarlo en productos
let ProductosParaVenta = [];
$("#cboBuscarProducto").on("select2:select",function(e){
    const data = e.params.data;
    
    let producto_encontrado = ProductosParaVenta.filter(p => p.idProducto == data.id)
    if(producto_encontrado.length > 0){
        $("#cboBuscarProducto").val("").trigger("change");
        toastr.warning("","El producto ya fue agregado");
        return false;
    }

    swal({
        title:data.marca,
        text:data.text,
        imageUrl:data.urlImagen,
        type:"input",
        showCancelButton: true,
        closeoOnConfirm: false,
        inputPlaceholder:"Ingrese cantidad"
    },
        function(valor){
            if(valor === false){
                return false;
            }

            if(valor === ""){
                toastr.warning("","Necesita ingresar la cantidad");
                return false;
            }

            if(isNaN(parseInt(valor))){
                toastr.warning("","Debe ingresar valor numerico");
                return false;
            }

            let producto = {
                idProducto:         data.id,
                marcaProducto:      data.marca,
                descripcionProducto:data.text,
                categoriaProducto:  data.categoria,
                cantidad:           parseInt(valor),
                precio:             data.precio.toString(),
                total:              (parseFloat(valor) * data.precio.toString())
            }

            ProductosParaVenta.push(producto);
            mostrarProductoPrecios();
            $("#cboBuscarProducto").val("").trigger("change");
            swal.close();
        }
    );

});

// Mostrar precios de productos en los inputs
function mostrarProductoPrecios(){
    let total = 0;
    let igv = 0;
    let subtotal = 0;
    let porcentaje = ValorImpuesto / 100;

    $("#tbProducto tbody").html("");
    ProductosParaVenta.forEach((item) => {
        total = total + parseFloat(item.total);
        $("#tbProducto tbody").append(
            $("<tr>").append(
                $("<td>").append(
                    $("<button>").addClass("btn btn-danger btn-eliminar btn-sm").append(
                        $("<i>").addClass("fas fa-trash-alt")
                    ).data("idProducto",item.idProducto)
                ),
                $("<td>").text(item.descripcionProducto),
                $("<td>").text(item.cantidad),
                $("<td>").text(item.precio),
                $("<td>").text(item.total)
            )
        );
    });

    subtotal = total/(1 + porcentaje);
    igv = total-subtotal;

    $("#txtSubTotal").val(subtotal.toFixed(2));
    $("#txtIGV").val(igv.toFixed(2));
    $("#txtTotal").val(total.toFixed(2));
}

// Funcion de eliminar productos y actualizar precios
$(document).on("click","button.btn-eliminar",function (){
    const _idProducto = $(this).data("idProducto");
    ProductosParaVenta = ProductosParaVenta.filter(p => p.idProducto != _idProducto);
    mostrarProductoPrecios();
});

// Funcion para terminar y guardar la venta
$("#btnTerminarVenta").click(function (){
    if(ProductosParaVenta.length < 1){
        toastr.warning("","Debe ingresar productos");
        return;
    }

    const vmDetalleVenta = ProductosParaVenta;
    console.log("vmDetalleVenta",vmDetalleVenta);
    const venta = {
        idTipoDocumentoVenta:$("#cboTipoDocumentoVenta").val(),
        documentoCliente:    $("#txtDocumentoCliente").val(),
        nombreCliente:       $("#txtNombreCliente").val(),
        subTotal:            $("#txtSubTotal").val(),
        impuestoTotal:       $("#txtIGV").val(),
        total:               $("#txtTotal").val(),
        DetalleVenta:        vmDetalleVenta
    }
    console.log("venta",venta)

   $("#btnTerminarVenta").LoadingOverlay("show");

    fetch("/Venta/RegistrarVenta",{
        method: "POST",
        headers: {"Content-Type":"application/json;charset=utf-8"},
        body: JSON.stringify(venta),
    }) 
    .then(response => {
        console.log("response",response);
        $("#btnTerminarVenta").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {
        console.log("/Venta/RegistrarVenta",responseJson.objeto);
        if(responseJson.estado){
            console.log("If:",responseJson);
            ProductosParaVenta = [];
            mostrarProductoPrecios();

            $("#txtDocumentoCliente").val("");
            $("#txtNombreCliente").val("");
            $("#cboTipoDocumentoVenta").val($("#cboTipoDocumentoVenta option:first").val());

            swal("Registrado!", `Numero Venta: ${responseJson.objeto.numeroVenta}`, "success");

        } else {
            // Checarlo mañana no guarda el dato y pasa directo al else
            swal("Lo sentimos!", "No se pudo registrar la venta", "error");
            console.log("else:",responseJson.objeto);
        }
    });

});