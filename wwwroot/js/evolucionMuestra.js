$(document).ready(function () {
    // $("#spinner").hide();
});

$("#tipoMuestraSelect").on("change", function () {


    //vacio tabla
    var idTipoMuestra = $(this).val();
    obtenerVaribles(idTipoMuestra);

})

function obtenerVaribles(idTipoMuestra) {

    $.ajax({
        url: '/muestras/ObtenerNombresVariables?idTipoMuestra=' + idTipoMuestra,
        method: 'GET'
    }).then(function (data) {
        console.log(data)


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