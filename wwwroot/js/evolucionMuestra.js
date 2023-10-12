$(document).ready(function () {
    // $("#spinner").hide();
    $("#fechaDesde").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });
    $("#fechaHasta").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });
    var vals = [
        [20, 20, 12, 15, 13, 13],
        [22, 22, 22, 45, 23, 11]
    ]


    //pintarGrafico("evolution-chart", "Evolution", ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"], vals);
    pintarGrafico("evolution-chart");
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


function pintarGrafico(destino) {

    var miCanvas = document.getElementById(destino).getContext('2d');
    /*
    datasetvalues = {
        labels: labels, // x-azis label values
        datasets: [DatasetMin] // y-axis
    };*/
    barChartOptions = {
        indexAxis: 'x',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
            title: {
                display: true,
                text: 'Variables'
            }
        },
        scales: {
            x: {
                barPercentage: 1,
                categoryPercentage: 0.6,
            },
            y: {
                barPercentage: 1,
                categoryPercentage: 0.6,
                ticks: {
                    beginAtZero: true
                }
            }
        }
    }
    // Define los datos para tres conjuntos de datos
    var datos = {
        labels: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo'],
        datasets: [
            {
                label: 'Conjunto de Datos 1',
                data: [10, 20, 15, 30, 25],
                borderColor: 'red',
                fill: false,
                tension: 0.1
            },
            {
                label: 'Conjunto de Datos 2',
                data: [5, 15, 10, 20, 30],
                borderColor: 'blue',
                fill: false,
            },
            {
                label: 'Conjunto de Datos 3',
                data: [25, 10, 30, 5, 15],
                borderColor: 'green',
                fill: false,
            },
        ],
    };


    // Crea el gráfico de líneas
    var miGrafico = new Chart(miCanvas, {
        type: 'line',
        data: datos,
        options: barChartOptions
    });
    return datasetvalues;
}