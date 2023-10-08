var dataTable;

$(document).ready(function () {
    //cargaTabla();
});

$("#btnBuscar").click(function () {
    
    var idTipoMuestra = document.querySelector("#selectipoMuestra").value;
    cargaTabla(idTipoMuestra);

})

function cargaTabla(idTipoMuestra) {


    $.ajax({
        url: '/informes/GetMuestrasYValoresJson?idTipoMuestra=' + idTipoMuestra,
        method: 'GET'
    }).then(function (data) {

                pintarCabeceraTabla(data.data.nombresVariables);

                $.each(data.data.listaMuestrasSalida, function (index, value) {
                    pintarLineaTabla(index, value, data.data.nombresVariables);
                   // console.log(index, value);
                });
                
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
