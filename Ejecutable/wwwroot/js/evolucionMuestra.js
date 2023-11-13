﻿$(document).ready(function () {
    // $("#spinner").hide();
    $("#fechaDesde").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });
    $("#fechaHasta").datepicker({ changeYear: true, defaultDate: 0, language: 'es', dateFormat: 'dd/mm/yy' });
    var vals = [
        [20, 20, 12, 15, 13, 13],
        [22, 22, 22, 45, 23, 11]
    ]


    //pintarGrafico("evolution-chart", "Evolution", ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"], vals);
    //pintarGrafico("evolution-chart");
    $('#btnEnviar').click(function (e) {
        //e.preventDefault(); // Evita la recarga de la página

        if ($('input[name="variables"]').is(':checked')) {
            validarFechas();
            mostrarGraficoEvolucion();
           
        } else {
            toastr.error("Selecciona al menos una variable");
        }

    });

});

$("#tipoMuestraSelect").on("change", function () {


    //vacio tabla
    $("#variables").text("");
    var idTipoMuestra = $(this).val();
    obtenerVaribles(idTipoMuestra);

})
/**
 * Obtiene los nombre de las variables de una muestra
 * @param {any} idTipoMuestra
 */
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
/**
 * Pinta la lista de variables en el formulario para seleccionar
 * @param {any} datos
 */
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
/**
 * Obtiene los datos para pintar el grafico principal (el que muestra la evolucion)
 */
function mostrarGraficoEvolucion() {



    // Obtén los datos del formulario
    var formData = $('#formDatosEvolucion').serialize();

    // Realiza la solicitud AJAX
    $.ajax({
        type: 'POST',
        url: '/muestras/ObtenerDatosEvolucion', // Reemplaza 'TuURL' con la URL del servidor
        data: formData,
        success: function (response) {
            // Maneja la respuesta del servidor
            console.log('pedirDatosGrafico() Solicitud exitosa:', response);

            if (Object.keys(response.data).length === 0) {
                toastr.error("No hay datos para mostrar");
                return false;
            } else {
                pintarGraficoEvolucion("evolution-chart", response);
                pedirDatosGraficoEstadistico();
            }

            
            // formatearDatosParaGrafico(response.data)
        },
        error: function (error) {
            // Maneja errores
            console.log('pedirDatosGrafico() Error en la solicitud:', error);
            toastr.error("Error al pedir datos para grafico evolucion");
        }
    });
}
/**
 * Pintar el grafico de evolucion
 * @param {any} destino
 * @param {any} datosServidor
 */
function pintarGraficoEvolucion(destino, datosServidor) {

    var labels = [];
    var datasets = []
    var nombreVariables = [];

    detroyCanvas(destino);

    
   
    $.each(datosServidor.data, function (nombreVariable, value) {
        // index son las variables
        //var data = []
        // dataset con los valores de la variable
        var dataset = {
            label: '',
            data: [],
            borderColor: generarColorAleatorio(),
            fill: false,
            tension: 0.1
        };
        var datasetMax = {
            label: nombreVariable + '-Max',
            data: [],
            borderColor: generarColorAleatorio(),
            fill: false,
            tension: 0.1
        };
        var datasetMin = {
            label: nombreVariable + '-Min',
            data: [],
            borderColor: generarColorAleatorio(),
            fill: false,
            tension: 0.1
        };
        dataset.label = nombreVariable
        nombreVariables.push(nombreVariable);

        //cargamos valores de referencia
        $.each(value, function (index2, value2) {
            // index2 son las fechas
            //console.log(index2, value2);
            dataset.data.push(value2["valor"]);
            // pintamos los valores de referencia
            if (datosServidor.valoresReferencia.hasOwnProperty(nombreVariable)) {

                datasetMin.data.push(datosServidor.valoresReferencia[nombreVariable][0]["minimo"])
                datasetMax.data.push(datosServidor.valoresReferencia[nombreVariable][0]["maximo"])
                console.log("index2: ", index2, datosServidor.valoresReferencia[nombreVariable][0])
            }
            else {

            }


        });
        datasets.push(dataset);
        // valores de referencia los pintamos si el checkbox esta marcado
        if ($('#pintarValorReferencia').is(":checked")) {
            datasets.push(datasetMin);
            datasets.push(datasetMax);
        }

        if (!datosServidor.valoresReferencia.hasOwnProperty(nombreVariable) && $('#pintarValorReferencia').is(":checked")) {
            toastr.warning("No se encontraron valores de referencia para la variable:" + nombreVariable)
        }


        //dataset con los maximos y minimos de la varialbe


    });


    console.log("datasets: ")
    console.log(datasets);

    $.each(datosServidor.data[nombreVariables[0]], function (index, value) {

        labels.push(value["fecha"]);
    })
    console.log("LABELS: ")
    console.log(labels);

    // Define los datos para tres conjuntos de datos
    var datos = {
        labels: labels,
        datasets: datasets
    };


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

    // Crea el gráfico de líneas
    var miGrafico = new Chart(miCanvas, {
        type: 'line',
        data: datos,
        options: barChartOptions
    });


    return true;

}
function generarColorAleatorio() {
    const color = '#' + Math.floor(Math.random() * 16777215).toString(16);
    return color;
}
function detroyCanvas(id) {
    var canvas = document.getElementById(id);
    // Verifica si ya existe un gráfico en el canvas
    if (canvas) {

        // Destruye el gráfico existente
        var chart = Chart.getChart(canvas);
        if (chart) {
            chart.destroy();
        }
    }
}
function pintarGraficoPrueba(destino) {

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
    //return datasetvalues;
}

// GRAFICO ESTADISTICO
function pedirDatosGraficoEstadistico() {

    $("#tipoMuestraSelect").val();
    $("#informacionEstadistica").text("");
    detroyCanvas("estadistico-chart");
    // Obtén los datos del formulario
    var formData = { "idCampo": 1, "idTipoMuestra": $("#tipoMuestraSelect").val() }
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

    var titulo = `<h2 class="text-primary mt-3">Estadisticas de las muestras en campo "${datosServidor.campo.nombre}" </h2>`;
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

    pintarGraficoEstadisticp('media-bar-chart', 'Media', Labels, valoresMedios);
    pintarGraficoEstadisticp('min-bar-chart', 'Minimos', Labels, valoresMinimos);
    pintarGraficoEstadisticp('max-bar-chart', 'Maximos', Labels, valoresMaximos);


    function pintarGraficoEstadisticp(idDestino, titulo, Labels, valores) {
        detroyCanvas(idDestino);



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

        var maximosChart = new Chart(
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
function validarFechas() {
    
    var fechaDesde = $("#fechaDesde").val();
    var fechaHasta = $("#fechaHasta").val();

    // Realiza la validación
    if (!fechaDesde || !fechaHasta) {
        // Una o ambas fechas son nulas o vacías
        alert("Ambas fechas son requeridas");

    } else {
        // Convierte las fechas a objetos Date para compararlas
        var fechaDesdeObj = new Date(fechaDesde);
        var fechaHastaObj = new Date(fechaHasta);

        // Compara las fechas
        if (fechaDesdeObj > fechaHastaObj) {
            // fechaDesde es mayor que fechaHasta
            alert("La fecha de inicio debe ser menor o igual a la fecha de fin");

        }
        // Si ambas validaciones pasan, el formulario se enviará normalmente
    }
}