document.addEventListener('DOMContentLoaded',function (){
    dockBar = document.getElementById('sidebar').ej2_instances[0];
    document.getElementById('sidebar-toggler').onclick = function () {
        dockBar.toggle();
    };
});