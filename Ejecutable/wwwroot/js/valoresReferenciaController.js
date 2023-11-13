$(document).ready(function () {




    $('#btnEnviar').click(function (e) {
        //e.preventDefault(); // Evita la recarga de la página

        if ($('input[name="variables"]').is(':checked')) {
            pedirDatosGrafico();
        } else {
            alert("Selecciona al menos una variable");
        }

    });

    $("#formulario").on("submit", function (e) {
        e.preventDefault()
        if (validarFormulario()) {
            //enviar formulario
            $(this).off("submit"); // Desactivar el controlador del evento de envío para evitar la recursión
            this.submit(); // Enviar el formulario
        }

    })

});

$("#tipoMuestraSelect").on("change", function () {


    //vacio tabla
    //$("#variables").text("");
    var idTipoMuestra = $(this).val();
    if (idTipoMuestra != "") {
        obtenerVaribles(idTipoMuestra);
    }


})

function obtenerVaribles(idTipoMuestra) {

    $.ajax({
        url: '/valoresReferencia/ObtenerNombresVariables?idTipoMuestra=' + idTipoMuestra,
        method: 'GET'
    }).then(function (data) {

        pintarVariables(data.data);
        console.log(data.data);

    })
        .fail(function (xhr, status, errorThrown) {
            alert("Error en la petición");
            console.log("Error: " + errorThrown);
            console.log("Status: " + status);
            console.dir(xhr);
        })
        .always(function (xhr, status) {
            // $("#spinner").addClass("d-none");

        });
}
function pintarVariables(datos) {

    if (datos.length == 0) {
        alert("No se han encontrado valores de referencia vacios, si quiere puede editar los existentes desde el listado")
        return;

    }

    $("#variableSelect").text = "";





    $.each(datos, function (index2, value2) {
        var nuevaOpcion = $("<option>");

        // Configurar las propiedades de la opción
        nuevaOpcion.attr("value", value2["id"]);
        nuevaOpcion.text(value2["nombre"]);


        console.log(value2["id"], value2["nombre"]);

        $("#variableSelect").append(nuevaOpcion);
    });


}

function validarFormulario() {
    var maximo = $("#maximo").val();
    var minimo = $("#minimo").val();
    var variableSelect = $("#variableSelect").val();
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
    if (variableSelect == "" || variableSelect == "0") {
        alert("Debe seleccionar una variable");
        return false;
    }
    return true;
}