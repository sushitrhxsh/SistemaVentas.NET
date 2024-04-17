let tablaData;
const MODELO_BASE = {
    idCategoria: 0,
    descripcion: "",
    esActivo: 1,
}

$(document).ready(function (){

    tablaData = $("#tbdata").DataTable({
        responsive: true,
        ajax: {
            url     :"/Categoria/Lista",
            type    : "GET",
            datatype: "json"
        },
        columns: [
            { data: "idCategoria", visible: false, searchable: false},           
            { data: "descripcion" },
            { data: "esActivo", render: renderStatus},
            {
                defaultContent: '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                                '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                orderable: false,
                searchable: false,
                width: "80px"
            }
        ],
        order: [[0, "desc"]],
        dom:"Bfrtip",
        buttons:[
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Categorias',
                exportOptions: {
                    columns: [1,2]
                }
            },'pageLength'
        ],
        language:{
            url:"https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

});

/* ------------------------------------- *
 *       Funciones de acciones           *
 * ------------------------------------- */
// Mostrar el btn y abrir el modal
$("#btnNuevo").click(function (){
    mostralModal();
});

// Guardar
$("#btnGuardar").click(function (){ 
    if($("#txtDescripcion").val().trim() == ""){
        toastr.warning("", "Debe completar el campo descripcion");
        $("#txtDescripcion").focus();
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idCategoria"] = parseInt($("#txtId").val());
    modelo["descripcion"] = $("#txtDescripcion").val();
    modelo["esActivo"]    = $("#cboEstado").val();
   
    $("#modalData").find(".modal-content").LoadingOverlay("show");

    if(modelo.idCategoria == 0){
        fetch("/Categoria/Crear",{
            method: "POST",
            headers: {"Content-Type":"application/json;charset=utf-8"},
            body: JSON.stringify(modelo)
        })
        .then(response => {
            $("#modalData").find(".modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if(responseJson.estado){
                tablaData.row.add(responseJson.objeto).draw(false);
                $("#modalData").modal("hide");
                
                swal("Listo!", "La categoria fue creada", "success");
            } else {
                swal("Lo sentimos!", responseJson.mensaje, "error");
            }
        });
    } else {
        fetch("/Categoria/Editar",{
            method: "PUT",
            headers: {"Content-Type":"application/json;charset=utf-8"},
            body: JSON.stringify(modelo)
        })
        .then(response => {
            $("#modalData").find(".modal-content").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if(responseJson.estado){
                tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                filaSeleccionada = null;
                $("#modalData").modal("hide");
                
                swal("Listo!", "La categoria fue modificada", "success");
            } else {
                swal("Lo sentimos!", responseJson.mensaje, "error");
            }
        });
    }

});

// Editar
let filaSeleccionada;
$("#tbdata tbody").on("click",".btn-editar", function (){
    if($(this).closest("tr").hasClass("child")){
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();
    
    mostralModal(data);
});

// Eliminar
$("#tbdata tbody").on("click",".btn-eliminar", function (){
    let fila;
    if($(this).closest("tr").hasClass("child")){
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");
    }

    const data = tablaData.row(fila).data();

    swal({
        title:"¿Estas seguro?",
        text:`Eliminar la categoria "${data.descripcion}"`,
        type:"warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeoOnConfirm: false,
        closeOnCancel: true
    },
        function(respuesta){
            if(respuesta){
                $(".showSweetAlert").LoadingOverlay("show");

                fetch(`/Categoria/Eliminar?IdCategoria=${data.idCategoria}`,{
                    method: "DELETE"
                })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json() : Promise.reject(response);
                })
                .then(responseJson => { 
                    if(responseJson.estado){
                        tablaData.row(fila).remove().draw();
                        swal("Listo!", "La categoria fue eliminada", "success");
                    } else {
                        swal("Lo sentimos!", responseJson.mensaje, "error");
                    }
                });
                //.finally(() => { // agregamos esto por el error
                //    $(".showSweetAlert").LoadingOverlay("hide");
                //});

            }
        }
    );
    
});


/* ----------------------------------------- *
 *       Funciones modals y renders          *
 * ----------------------------------------- */
// Funcion para mostrar el nodal con sus datos llenos
function mostralModal(modelo = MODELO_BASE){
    $("#txtId").val(modelo.idCategoria);
    $("#txtDescripcion").val(modelo.descripcion);
    $("#cboEstado").val(modelo.esActivo);

    $("#modalData").modal("show");
}

// Funcion para renderizar los span
function renderStatus(data){
    if (data === 1)
        return '<span class="badge badge-success">Activo</span>';
    else
        return '<span class="badge badge-danger">No Activo</span>';
}