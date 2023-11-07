$(document).ready(function () {


    $("#formulario").on("submit", function (e) {
        e.preventDefault()
        if (validarFormulario()) {
            //enviar formulario
            $(this).off("submit"); // Desactivar el controlador del evento de envío para evitar la recursión
            this.submit(); // Enviar el formulario
        }

    })

});

function validarFormulario() {
    var maximo = $("#maximo").val();
    var minimo = $("#minimo").val();

    if (minimo != "" && maximo != "") {
        if (minimo > maximo) {
            alert("El valor mínimo no puede ser mayor que el máximo");
            return false;
        }
    }
    if (minimo == "") {
        alert("El valor mínimo no puede vacio");
        return false;
    }
    if (maximo == "") {
        alert("El valor maximo no puede vacio");
        return false;
    }

    return true;
}