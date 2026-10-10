(() => {
    const elemento = document.getElementById("graficaMovimientos");
    const datos = window.datosTablero;
    if (!elemento || !datos || typeof Chart === "undefined") return;

    new Chart(elemento, {
        type: "bar",
        data: {
            labels: datos.meses,
            datasets: [
                { label: "Ventas", data: datos.ventas, backgroundColor: "rgba(49, 87, 213, .82)", borderRadius: 5, maxBarThickness: 28 },
                { label: "Compras", data: datos.compras, backgroundColor: "rgba(37, 169, 125, .72)", borderRadius: 5, maxBarThickness: 28 }
            ]
        },
        options: {
            maintainAspectRatio: false,
            responsive: true,
            interaction: { mode: "index", intersect: false },
            plugins: {
                legend: { position: "bottom", labels: { usePointStyle: true, pointStyle: "circle", boxWidth: 7, padding: 18 } },
                tooltip: { callbacks: { label: contexto => `${contexto.dataset.label}: Bs ${contexto.parsed.y.toLocaleString("es-BO", { minimumFractionDigits: 2 })}` } }
            },
            scales: {
                x: { grid: { display: false }, border: { display: false }, ticks: { color: "#8a92a3", font: { size: 10 } } },
                y: { beginAtZero: true, border: { display: false }, grid: { color: "#edf0f5" }, ticks: { color: "#8a92a3", font: { size: 9 }, callback: valor => `Bs ${valor}` } }
            }
        }
    });
})();
