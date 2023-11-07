﻿var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblValoresReferencia").DataTable({
        "ajax": {
            "url": "/ValoresReferencia/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [

            
            { "data": "nombreVariableMuestra.nombre", "width": "20%" },
            { "data": "minimo", "width": "20%" },
            { "data": "maximo", "width": "20%" },


            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/ValoresReferencia/Edit/${data}" class="btn btn-primary text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-edit"></i>Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete("/ValoresReferencia/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i>Borrar
                                </a>
                            </div>
                            `;
                }, "width": "30%"
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}