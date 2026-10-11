(() => {
    const boton = document.querySelector('.boton-menu-movil');
    const barra = document.querySelector('#barra-lateral');
    const fondo = document.querySelector('.fondo-menu');

    if (!boton || !barra) return;

    const esMovil = window.matchMedia('(max-width: 760px)');

    function actualizarEstado() {
        const abierto = esMovil.matches
            ? document.body.classList.contains('menu-abierto')
            : !document.body.classList.contains('barra-colapsada');

        boton.setAttribute('aria-expanded', String(abierto));
        boton.setAttribute('aria-label', abierto ? 'Contraer barra lateral' : 'Desplegar barra lateral');
    }

    function alternarMenu() {
        if (esMovil.matches) {
            document.body.classList.toggle('menu-abierto');
        } else {
            document.body.classList.toggle('barra-colapsada');
        }
        actualizarEstado();
    }

    function cerrarMenuMovil() {
        document.body.classList.remove('menu-abierto');
        actualizarEstado();
    }

    boton.addEventListener('click', alternarMenu);
    fondo?.addEventListener('click', cerrarMenuMovil);
    barra.addEventListener('click', evento => {
        if (esMovil.matches && evento.target.closest('a')) cerrarMenuMovil();
    });
    document.addEventListener('keydown', evento => {
        if (evento.key === 'Escape' && esMovil.matches) cerrarMenuMovil();
    });
    esMovil.addEventListener('change', () => {
        document.body.classList.remove('menu-abierto');
        actualizarEstado();
    });

    actualizarEstado();
})();
