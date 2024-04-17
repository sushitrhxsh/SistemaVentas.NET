$(document).ready(function (){
    $("div.container-fluid").LoadingOverlay("show");

    fetch("/DashBoard/ObtenerResumen")
    .then(response => {
        $("div.container-fluid").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {    
        if(responseJson.estado){
            // Mostrar datos en las trajetas
            let d = responseJson.objeto;

            $("#totalVenta").text(d.totalVentas);
            $("#totalIngresos").text(d.totalIngresos);
            $("#totalProductos").text(d.totalProductos);
            $("#totalCategorias").text(d.totalCategorias);
            
            // Obtener los datos para los graficos
            let barChartLabels;
            let barChartData;
            if(d.ventasUltimaSemana.length > 0){
                barChartLabels = d.ventasUltimaSemana.map((item) => { return item.fecha });
                barChartData = d.ventasUltimaSemana.map((item) => { return item.total });
            } else {
                barChartLabels = ["sin resultados"];
                barChartData = [0];
            }

            // Obtener datos para el grafico de pie
            let pieChartLabels;
            let pieChartData;
            if(d.productosTopUltimaSemana.length > 0){
                pieChartLabels = d.productosTopUltimaSemana.map((item) => { return item.producto });
                pieChartData = d.productosTopUltimaSemana.map((item) => { return item.cantidad });
            } else {
                pieChartLabels = ["sin resultados"];
                pieChartData = [0];
            }

            // Configuracion para el grafico de barras
            let controlVenta = document.getElementById("chartVentas");
            let myBarChart = new Chart(controlVenta, {
                type: 'bar',
                data: {
                    labels: barChartLabels,
                    datasets: [{
                        label: "Cantidad",
                        backgroundColor: "#4e73df",
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: barChartData,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    },
                    scales: {
                    xAxes: [{
                        gridLines: {
                            display: false,
                            drawBorder: false
                        },
                        maxBarThickness: 50,
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            maxTicksLimit: 5
                        }
                    }],
                    },
                }
            });

            // Configuracion para el grafico de pie
            let controlProducto = document.getElementById("chartProductos");
            let myPieChart = new Chart(controlProducto, {
            type: 'doughnut',
            data: {
                labels: pieChartLabels,
                datasets: [{
                data: pieChartData,
                backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc',"#FF785B"],
                hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf',"#FF5733"],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
                }],
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                },
                legend: {
                display: true
                },
                cutoutPercentage: 80,
            },
            });

        } else {
            swal("Lo sentimos!", responseJson.mensaje, "error");
        }
    });

});