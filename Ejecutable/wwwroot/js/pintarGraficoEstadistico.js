$(document).ready(function () {

    $('#btnEnviar').click(function (e) {
        e.preventDefault(); // Evita la recarga de la página

        pedirDatosGraficoEstadistico();

    });

});

var myChart;
var maxMedMinChart;
function pedirDatosGraficoEstadistico() {

    $("#tipoMuestraSelect").val();
    $("#informacionEstadistica").text("");
    //detroyCanvas("estadistico-chart");
    // Obtén los datos del formulario
    var formData = { "idCampo": $("#campo").val(), "idTipoMuestra": $("#tipoMuestra").val() }
    // Realiza la solicitud AJAX
    $.ajax({
        type: 'POST',
        url: '/muestras/DatosGraficaPorCampoYTipoMuestra', // Reemplaza 'TuURL' con la URL del servidor
        data: formData,
        success: function (response) {
            // Maneja la respuesta del servidor
            console.log('Solicitud exitosa:', response);
            pintarGraficoEstadistico(response.valoresPorCampoYTipoMuestraVM);
            // formatearDatosParaGrafico(response.data)
        },
        error: function (error) {
            // Maneja errores
            toastr.error("Error al pedir datos para grafico estadistico");
        }
    });


}


function pintarGraficoEstadistico(datosServidor) {

    var titulo = `<h2 class="text-primary mt-3">Estadísticas de las muestras "${datosServidor.tipoMuestra.nombre}" </h2>`;
    var subtitulo = "<h3>Cantidad muestras analizadas: " + datosServidor.cantidadMuestras + "</h3>";
    $("#informacionEstadistica").append(titulo);
    $("#informacionEstadistica").append(subtitulo);


    var valoresMedios = [];
    var valoresMinimos = [];
    var valoresMaximos = [];
    var Labels = [];
    console.log("datosServidor: ", datosServidor);

    $.each(datosServidor.media, function (index, value) {
        valoresMedios.push(value["valor"]);
        Labels.push(value["nombre"]);
    });

    $.each(datosServidor.minimo, function (index, value) {
        valoresMinimos.push(value["valor"]);

    });
    $.each(datosServidor.maximo, function (index, value) {
        valoresMaximos.push(value["valor"]);

    });


    console.log("valoresMedios:", valoresMedios);
    console.log("Labels:", Labels);
    for (var i = 0; i < valoresMedios.length; i++) {
        valoresMedios[i] = parseFloat(valoresMedios[i]);
        valoresMinimos[i] = parseFloat(valoresMinimos[i]);
        valoresMaximos[i] = parseFloat(valoresMaximos[i]);
    }
    console.log("valoresMedios:", valoresMedios);
    console.log("Labels:", Labels);




    //pintarGraficoEstadisticp('media-bar-chart', 'Media', Labels, valoresMedios);
    //pintarGraficoEstadisticp('min-bar-chart', 'Minimos', Labels, valoresMinimos);
    //pintarGraficoEstadisticp('max-bar-chart', 'Maximos', Labels, valoresMaximos);
    pintarMaxMedMin(Labels, valoresMedios, valoresMinimos, valoresMaximos);
    function pintarMaxMedMin(Labels, valoresMedios, valoresMinimos, valoresMaximos) {
        //detroyCanvas("media-bar-chart");
        if (maxMedMinChart != null) {
            maxMedMinChart.destroy();
        }
         maxMedMinChart = new Chart(
            document.getElementById("media-bar-chart").getContext('2d'), {
            type: 'bar',
            data: {
                labels: Labels,
                datasets: [
                    {
                        label: 'Minimo',
                        data: valoresMinimos,
                        backgroundColor: 'rgba(255, 205, 86, 0.5)',
                        borderColor: 'rgba(255, 205, 86, 1)',
                        borderWidth: 1,
                        fill: false,
                    },
                    {
                        label: 'Media',
                        data: valoresMedios,
                        backgroundColor: 'rgba(75, 192, 192, 0.5)',
                        borderColor: 'rgba(75, 192, 192, 1)',
                        borderWidth: 1,
                        fill: false,
                        tension: 0.1
                    },

                    {
                        label: 'Maximo',
                        data: valoresMaximos,
                        backgroundColor: 'rgba(255, 99, 132, 0.5)',
                        borderColor: 'rgba(255, 99, 132, 1)',
                        borderWidth: 1,
                        fill: false,
                    },
                ],
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                categoryPercentage: 0.8,
                //  barPercentage: 0.5,
                scales: {
                    x: {
                        
                        position: "bottom", // Posición del eje x,
                        

                    },
                    y: {
                        beginAtZero: true, // Comenzar en cero en el eje y
                    },
                },
            }
        });
    }

    function pintarGraficoEstadisticp(idDestino, titulo, Labels, valores) {
        //detroyCanvas(idDestino);

        if (myChart != null) {
            myChart.destroy();
        }

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

        myChart = new Chart(
            document.getElementById(idDestino).getContext('2d'), {
            type: 'bar',
            data: obtenerDatasetValues(titulo, Labels, valores),
            options: barChartOptions
        });
    }

    function obtenerDatasetValues(label = "Titulo", labels, valores) {
        var DatasetMin = {
            label: label,
            data: valores,
            borderWidth: 1,
            lineTension: 0,
        };
        datasetvalues = {
            labels: labels, // x-azis label values
            datasets: [DatasetMin] // y-axis
        };
        return datasetvalues;
    }

}


