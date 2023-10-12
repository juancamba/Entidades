$(document).ready(function () {
    // $("#spinner").hide();
    $("#fechaDesde").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });
    $("#fechaHasta").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });

    $('#formDatosEvolucion').submit(function (e) {
        e.preventDefault(); // Evita la recarga de la página

        // Obtén los datos del formulario
        var formData = $(this).serialize();

        // Realiza la solicitud AJAX
        $.ajax({
            type: 'POST',
            url: '/muestras/ObtenerDatosEvolucion', // Reemplaza 'TuURL' con la URL del servidor
            data: formData,
            success: function (response) {
                // Maneja la respuesta del servidor
                console.log('Solicitud exitosa:', response);
            },
            error: function (error) {
                // Maneja errores
                console.log('Error en la solicitud:', error);
            }
        });
    });

});

$("#tipoMuestraSelect").on("change", function () {


    //vacio tabla
    $("#variables").text("");
    var idTipoMuestra = $(this).val();
    obtenerVaribles(idTipoMuestra);

})

function obtenerVaribles(idTipoMuestra) {

    $.ajax({
        url: '/muestras/ObtenerNombresVariables?idTipoMuestra=' + idTipoMuestra,
        method: 'GET'
    }).then(function (data) {

        pintarVariables(data.data);

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
    var html = '<ul class="list-group">';

    $.each(datos, function (index2, value2) {
        html += '<li class="list-group-item">';
        html += `<input class="form-check-input me-1" type="checkbox" value="${value2["id"]}" name=variables id="var${value2["id"]}">`;
        html += `<label class="form-check-label stretched-link" for="var${value2["id"]}">${value2["nombre"]}</label>`;
        //<input class="form-check-input me-1" type="checkbox" value="" id="firstCheckboxStretched">
        html += '</li>';
        console.log(value2["id"], value2["nombre"]);


    });
    html += '</ul>';
    console.log(html);
    $("#variables").append(html);
}