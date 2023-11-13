// esto es para los alerts bonitos
toastr.options = {
    "closeButton": true,
    "progressBar": true,
    "positionClass": "toast-top-right", // Puedes ajustar la posición
    "showDuration": "300", // Duración de la animación de entrada
    "hideDuration": "1000", // Duración de la animación de salida
    "timeOut": "5000", // Tiempo que se mostrará el aviso (en milisegundos)
    "extendedTimeOut": "1000" // Tiempo adicional para mostrar el aviso si el usuario interactúa con él
};
// lo llamamos con toastr.success('mensaje') o toastr.error('mensaje') o toastr.warning('mensaje')