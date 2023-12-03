$(document).ready(function () {

    $('#btnEnviar').click(function (e) {
        //e.preventDefault(); // Evita la recarga de la página

        if ($('input[name="variables"]').is(':checked')) {
            mostrarGraficoEvolucion();

        } else {
            toastr.error("Selecciona al menos una variable");
        }

    });
    // cargamos las VARIABLES EN LOS SELECT
    var idTipoMuestra = $("#tipoMuestraSelect").val();
    if (idTipoMuestra != 0) {
        obtenerVaribles(idTipoMuestra);
    }

  
});
var myChart;
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
                //pedirDatosGraficoEstadistico();
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
    if (myChart != null) {
        myChart.destroy();
    }
    
    //detroyCanvas(destino);

    //cargamos fechas en labels
    /*$.each(datosServidor.informacionFechas.fechasCompletas, function (index, value) {

        labels.push(value);
    })
    console.log("LABELS: ")
    console.log(labels);
    */
    $.each(datosServidor.data, function (nombreVariable, value) {
        // index son las variables

        // dataset con los valores de la variable

        var colorVariable = generarColorAleatorio();
        var dataset = {
            label: '',
            data: [],
            borderColor: colorVariable,
            hoverBackgroundColor: colorVariable,
            fill: false,
            pointRadius: 5, // Tamaño de los puntos
            pointHoverRadius: 8, // Tamaño de los puntos al pasar el ratón
            tension: 0.1
        };
        var datasetMax = {
            label: nombreVariable + '-Max',
            data: [],
            borderColor: generarColorAleatorio(),
            fill: false,
            pointRadius: 5, // Tamaño de los puntos
            pointHoverRadius: 8, // Tamaño de los puntos al pasar el ratón
            tension: 0.1
        };
        var datasetMin = {
            label: nombreVariable + '-Min',
            data: [],
            borderColor: generarColorAleatorio(),
            fill: false,
            pointRadius: 5, // Tamaño de los puntos
            pointHoverRadius: 8, // Tamaño de los puntos al pasar el ratón
            tension: 0.1
        };
        dataset.label = nombreVariable
        nombreVariables.push(nombreVariable);

        console.log("nombreVariable: ", nombreVariable);

        //cargamos valores de referencia
        $.each(value, function (index2, value2) {
            // index2 son las fechas
            console.log(value, index2, value2["valor"], value2["fecha"]);

           // dataset.data.push(value2["valor"]);
            var m = { x: new Date(value2["fecha"]), y: value2["valor"] };
            dataset.data.push(m)


            // pintamos los valores de referencia
            if (datosServidor.valoresReferencia.hasOwnProperty(nombreVariable)) {

                datasetMin.data.push({ x: new Date(value2["fecha"]), y: datosServidor.valoresReferencia[nombreVariable][0]["minimo"] })
                datasetMax.data.push({ x: new Date(value2["fecha"]), y: datosServidor.valoresReferencia[nombreVariable][0]["maximo"] })
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
    console.log(datasets)

    //console.log("FECHAS ")
    //console.log(datosServidor.informacionFechas);


    // Define los datos para tres conjuntos de datos
    var datos = {
       // labels: labels,
        datasets: datasets
    };


    var miCanvas = document.getElementById(destino).getContext('2d');

      
    var config = {
       responsive: false,
        maintainAspectRatio: false,
        type: "line",
        data: datos,
        options: {
            //events: null,
            tooltips: {
                enabled: true, // Habilita los tooltips
                mode: 'index', // Modo de interacción: 'index' activará solo el evento al pasar el ratón por un punto
                intersect: true // Intersecta con los elementos (puntos) del gráfico
            },
            hover: {
                mode: 'index',
                intersect: true
            },
            events: ['click', 'mousemove'], // Puedes anular otros eventos aquí
            scales: {
                xAxes: [
                    {
                        type: "time",
                        time: {
                            displayFormats: {
                                'month': 'DD/MM/YYYY',
                            },
                            tooltipFormat: 'DD/MM/YYYY'
                        },
                        scaleLabel: {
                            display: true,
                            labelString: "Fecha",
                        },
                        ticks: {
                            source: 'auto',
                            autoSkip: true,
                            maxTicksLimit: 15, // Puedes ajustar este valor según tus necesidades
                        },
                    },
                ],
                yAxes: [
                    {
                        scaleLabel: {
                            display: true,
                            labelString: "Valor",
                        },
                        ticks: {
                            beginAtZero: true, // Asegura que el eje y comience en cero
                        },
                    },
                ],
            },
        },
    };

    myChart = new Chart(miCanvas, config);
   // myChart.destroy();

    return true;

}
function generarColorAleatorio() {
    const color = '#' + Math.floor(Math.random() * 16777215).toString(16);
    return color;
}
function destroyCanvas() {

}
function detroyCanvasOld(id) {
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

