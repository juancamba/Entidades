var dataTable;

$(document).ready(function () {
   // $("#spinner").hide();
});

$("#btnBuscar").click(function () {
    
    var idTipoMuestra = document.querySelector("#selectipoMuestra").value;
    //vacio tabla
    $("#muestras").text("");
    cargaTabla(idTipoMuestra);

})

function cargaTabla(idTipoMuestra) {

    $("#spinner").removeClass("d-none");
    $.ajax({
        url: '/informes/GetMuestrasYValoresJson?idTipoMuestra=' + idTipoMuestra,
        method: 'GET'
    }).then(function (data) {
        if (data.data.listaMuestrasSalida.length == 0) {
            alert("no hay datos para mostrar")
        }
        else {
            pintarCabeceraTabla(data.data.nombresVariables);

            $.each(data.data.listaMuestrasSalida, function (index, value) {
                pintarLineaTabla(index, value, data.data.nombresVariables);
                // console.log(index, value);
            });
        }
               
                
    })
    .fail(function (xhr, status, errorThrown) {
            alert("Error en la petición");
            console.log("Error: " + errorThrown);
            console.log("Status: " + status);
            console.dir(xhr);
     })
    .always(function (xhr, status) {
        $("#spinner").addClass("d-none");
        
    });
}
function pintarLineaTabla(index, valor, variable) {
    var htmlTags = '<tr>'
    var lineaTabla = { 'IdMuestra':'', 'IdEntidad':'','Fecha':''};
    for (var i = 0; i < variable.length; i++) {
        lineaTabla[variable[i]] = '';
    }


    $.each(valor, function (index2, value2) {
        //console.log(index2, value2);
        lineaTabla[index2] = value2;


    })

    $.each(lineaTabla,function (index2, value2){
        htmlTags += `<td data-variable=${index2}>${value2}</td>`
    })
 
    htmlTags += '</tr>'
    $("#muestras").append(htmlTags);
    console.log(lineaTabla);
}
function pintarCabeceraTabla(valor) {
    
    var htmlTags = '<tr><thead><th>IDMuestra</th><th>IdEntidad</th><th>Fecha</th>';
    for (var i = 0; i < valor.length; i++) {
        
        htmlTags += `<th data-variable=${valor[i]}>${valor[i]}</th>`
        }
        
    htmlTags += '</thead></tr>'
    
    $("#muestras").append(htmlTags);
    //console.log(htmlTags);
}
