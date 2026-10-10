(() => {
    const lista = document.getElementById("listaDetalles");
    const plantilla = document.getElementById("plantillaDetalle");
    const formato = new Intl.NumberFormat("es-BO", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    if (!lista || !plantilla) return;

    const recalcular = () => {
        let total = 0;
        lista.querySelectorAll(".fila-detalle").forEach(fila => {
            const selector = fila.querySelector(".selector-producto");
            const cantidad = Number.parseInt(fila.querySelector(".entrada-cantidad").value, 10) || 0;
            const precio = Number.parseFloat(selector.selectedOptions[0]?.dataset.precio || "0") || 0;
            const subtotal = cantidad * precio;
            total += subtotal;
            fila.querySelector(".subtotal-detalle").textContent = `Bs ${formato.format(subtotal)}`;
        });
        document.getElementById("totalVenta").textContent = `Bs ${formato.format(total)}`;
    };

    document.getElementById("agregarDetalle").addEventListener("click", () => {
        const indice = lista.querySelectorAll(".fila-detalle").length;
        const contenido = plantilla.innerHTML.replaceAll("__indice__", String(indice));
        lista.insertAdjacentHTML("beforeend", contenido);
        recalcular();
    });
    lista.addEventListener("change", recalcular);
    lista.addEventListener("input", recalcular);
    lista.addEventListener("click", evento => {
        if (!evento.target.closest(".boton-quitar")) return;
        const filas = lista.querySelectorAll(".fila-detalle");
        if (filas.length <= 1) {
            filas[0].querySelector(".selector-producto").value = "0";
            filas[0].querySelector(".entrada-cantidad").value = "1";
        } else {
            evento.target.closest(".fila-detalle").remove();
        }
        lista.querySelectorAll(".fila-detalle").forEach((fila, indice) => {
            fila.querySelector(".selector-producto").name = `Detalles[${indice}].id_producto`;
            fila.querySelector(".entrada-cantidad").name = `Detalles[${indice}].cantidad`;
        });
        recalcular();
    });
    recalcular();
})();
