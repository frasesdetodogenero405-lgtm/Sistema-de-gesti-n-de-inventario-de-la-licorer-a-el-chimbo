document.addEventListener("click", (evento) => {
    const boton = evento.target.closest("[data-mostrar-contrasena]");
    if (!boton) return;

    const campo = boton.parentElement.querySelector("input");
    if (!campo) return;

    const mostrar = campo.type === "password";
    campo.type = mostrar ? "text" : "password";
    boton.setAttribute("aria-pressed", String(mostrar));
    boton.setAttribute("aria-label", mostrar ? "Ocultar contraseña" : "Mostrar contraseña");
});
