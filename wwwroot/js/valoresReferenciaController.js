$(document).ready(function () {




    $('#btnEnviar').click(function (e) {
        //e.preventDefault(); // Evita la recarga de la página

        if ($('input[name="variables"]').is(':checked')) {
            pedirDatosGrafico();
        } else {
            alert("Selecciona al menos una variable");
        }

    });

});

$("#tipoMuestraSelect").on("change", function () {


    //vacio tabla
    //$("#variables").text("");
    var idTipoMuestra = $(this).val();
    obtenerVaribles(idTipoMuestra);

})

function obtenerVaribles(idTipoMuestra) {

    $.ajax({
        url: '/muestras/ObtenerNombresVariables?idTipoMuestra=' + idTipoMuestra,
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

    $("#variableSelect").text = "";

    

    

    $.each(datos, function (index2, value2) {
        var nuevaOpcion = $("<option>");

        // Configurar las propiedades de la opción
        nuevaOpcion.attr("value",  value2["id"]);
        nuevaOpcion.text(value2["nombre"]);


        console.log(value2["id"], value2["nombre"]);

        $("#variableSelect").append(nuevaOpcion);
    });
    
   
}