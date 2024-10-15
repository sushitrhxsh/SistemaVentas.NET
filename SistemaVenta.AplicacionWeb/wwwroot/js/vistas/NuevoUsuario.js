const MODELO_BASE = {
    idUsuario: 0,
    nombre:    "",
    correo:    "",
    telefono:  "",
    idRol:     0,
    esActivo:  1,
    urlFoto:   ""
}

/* ------------------------------------- *
 *       Funciones de acciones           *
 * ------------------------------------- */
// Enviar datos del form
$("form").on("submit", function (event){
    event.preventDefault();

    const inputs = $(".input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() === "")

    if(inputs_sin_valor.length > 0){
        const mensaje = `Debe completar el campo: "${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje);
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus();
        
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["IdUsuario"] = parseInt($("#txtId").val());
    modelo["nombre"]    = $("#txtNombre").val();
    modelo["correo"]    = $("#txtCorreo").val();
    modelo["telefono"]  = $("#txtTelefono").val();
    modelo["idRol"]     = $("#cboRol").val();
    modelo["esActivo"]  = $("#cboEstado").val();
    const inputFoto = document.getElementById("txtFoto");

    const formData = new FormData();
    formData.append("foto",inputFoto.files[0]);
    formData.append("modelo",JSON.stringify(modelo));

    $("form").LoadingOverlay("show");

    if(modelo.idUsuario == 0){
        fetch("/Acceso/Registrar", {
            method: "POST",
            body:   formData
        })
        .then(response => {
            $("form").LoadingOverlay("hide");
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if(responseJson.estado){
                swal("Listo!", "El usuario fue creado", "success");
                $("form")[0].reset();
            } else {
                swal("Lo sentimos!", responseJson.mensaje, "error");
            }
        });
    }

});