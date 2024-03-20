let tablaData;
$(document).ready(function (){

    $.datepicker.setDefaults($.datepicker.regional["es"]);

    $("#txtFechaInicio").datepicker({ dateFormat: "dd/mm/yy" });
    $("#txtFechaFin").datepicker({ dateFormat: "dd/mm/yy" });

    tablaData = $("#tbdata").DataTable({
        responsive: true,
        ajax: {
            url     :"/Reporte/ReporteVenta?fechaInicio=01/01/2024&fechaFin=01/01/2024",
            type    : "GET",
            datatype: "json"
        },
        columns: [          
            { data: "fechaRegistro" },
            { data: "numeroVenta" },
            { data: "tipoDocumento" },
            { data: "documentoCliente" },
            { data: "nombreCliente" },
            { data: "subTotalVenta" },
            { data: "impuestoTotalVenta" },
            { data: "totalVenta" },
            { data: "producto" },
            { data: "cantidad" },
            { data: "precio" },
            { data: "total" },
        ],
        order: [[0, "desc"]],
        dom:"Bfrtip",
        buttons:[
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Ventas',
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
// Funcion de boton de busqueda por fechas
$("#btnBuscar").click(function (){

    if($("#txtFechaInicio").val().trim() == "" || $("#txtFechaFin").val().trim() == ""){
        toastr.warning("","Debe de ingresar la fecha de inicio y fin");
    }

    let fechaInicio = $("#txtFechaInicio").val().trim();
    let fechaFin = $("#txtFechaFin").val().trim();

    let nuevaUrl = `/Reporte/ReporteVenta?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;

    tablaData.ajax.url(nuevaUrl).load();

});