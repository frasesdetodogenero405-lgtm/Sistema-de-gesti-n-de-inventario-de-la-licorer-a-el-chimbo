(() => {
    const lista = document.getElementById("listaDetalles");
    const plantilla = document.getElementById("plantillaDetalle");
    const formato = new Intl.NumberFormat("es-BO", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    if (!lista || !plantilla) return;

    const recalcular = () => {
        let total = 0;
        lista.querySelectorAll(".fila-detalle").forEach(fila => {
            const cantidad = Number.parseInt(fila.querySelector(".entrada-cantidad").value, 10) || 0;
            const costo = Number.parseFloat(fila.querySelector(".entrada-compra").value) || 0;
            const subtotal = cantidad * costo;
            total += subtotal;
            fila.querySelector(".subtotal-detalle").textContent = `Bs ${formato.format(subtotal)}`;
        });
        document.getElementById("totalCompra").textContent = `Bs ${formato.format(total)}`;
    };

    document.getElementById("agregarDetalle").addEventListener("click", () => {
        const indice = lista.querySelectorAll(".fila-detalle").length;
        lista.insertAdjacentHTML("beforeend", plantilla.innerHTML.replaceAll("__indice__", String(indice)));
        recalcular();
    });
    lista.addEventListener("change", recalcular);
    lista.addEventListener("input", recalcular);
    lista.addEventListener("click", evento => {
        if (!evento.target.closest(".boton-quitar")) return;
        const filas = lista.querySelectorAll(".fila-detalle");
        if (filas.length <= 1) {
            filas[0].querySelectorAll("input").forEach(campo => campo.value = "");
            filas[0].querySelector(".entrada-cantidad").value = "1";
            filas[0].querySelector(".selector-producto").value = "0";
        } else {
            evento.target.closest(".fila-detalle").remove();
        }
        lista.querySelectorAll(".fila-detalle").forEach((fila, indice) => {
            fila.querySelector(".selector-producto").name = `Detalles[${indice}].id_producto`;
            fila.querySelector(".entrada-cantidad").name = `Detalles[${indice}].cantidad`;
            fila.querySelector(".entrada-compra").name = `Detalles[${indice}].precio_compra`;
            fila.querySelector(".entrada-venta").name = `Detalles[${indice}].precio_venta`;
        });
        recalcular();
    });
    recalcular();
})();
