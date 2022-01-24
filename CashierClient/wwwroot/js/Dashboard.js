$(document).ready(function () {
    $.ajax({
        url: "/users/getCounter",
        type: "GET",
        success: function (response) {
            console.log(response);
            document.getElementById("cashiercount").innerHTML = response.cashier + " Data";
            document.getElementById("suppliercount").innerHTML = response.supplier + " Data";
            document.getElementById("itemcount").innerHTML = response.items + " Data";
        },
        error: function (response) {
            console.log(response);
        }
    })


    $.ajax({
        url: "/users/GraphUser",
        type: "GET",
    }).done((result) => {
        console.log(result);
        var options = {
            chart: {
                type: "donut",
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                        reset: true | '<img src="/static/icons/reset.png" width="20">',
                        customIcons: []
                    },
                    export: {
                        csv: {
                            filename: "Grafik-payment",
                            columnDelimiter: ',',
                            headerCategory: 'Payment',
                            headerValue: 'Value',
                            dateFormatter(timestamp) {
                                return new Date(timestamp).toDateString()
                            }
                        },
                        svg: {
                            filename: "Grafik-Payment",
                        },
                        png: {
                            filename: "Grafik-Payment",
                        }
                    },
                    autoSelected: 'zoom'
                },
            },
            plotOptions: {
                pie: {
                    donut: {
                        labels: {
                            show: true,
                            name: {
                                show: true,
                            },
                            value: {
                                show: true,
                            },
                            total: {
                                show: true,
                                label: 'Total Payment Type',
                                formatter: function (w) {
                                    return w.globals.seriesTotals.reduce((a, b) => {
                                        return a + b
                                    }, 0)
                                }
                            }
                        }
                    }
                }

            },

            series: result.series,
            labels: result.label,
            colors: ['#417CFF', '#41A745']

        };
        var chart = new ApexCharts(document.querySelector("#gender_payment"), options);
        chart.render();

    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan Pada Chart Gender',
            'error'
        )
    })



    //status request 

    $.ajax({
        url: "/users/GraphUser2",
        type: "GET",
    }).done((result) => {
        console.log(result);
        var options = {
            chart: {
                type: "donut",
                toolbar: {
                    show: true,
                    offsetX: 0,
                    offsetY: 0,
                    tools: {
                        download: true,
                        selection: true,
                        zoom: true,
                        zoomin: true,
                        zoomout: true,
                        pan: true,
                        reset: true | '<img src="/static/icons/reset.png" width="20">',
                        customIcons: []
                    },
                    export: {
                        csv: {
                            filename: "Grafik-Request",
                            columnDelimiter: ',',
                            headerCategory: 'Request',
                            headerValue: 'Value',
                            dateFormatter(timestamp) {
                                return new Date(timestamp).toDateString()
                            }
                        },
                        svg: {
                            filename: "Grafik-Request",
                        },
                        png: {
                            filename: "Grafik-Request",
                        }
                    },
                    autoSelected: 'zoom'
                },
            },
            plotOptions: {
                pie: {
                    donut: {
                        labels: {
                            show: true,
                            name: {
                                show: true,
                            },
                            value: {
                                show: true,
                            },
                            total: {
                                show: true,
                                label: 'Total Status Request',
                                formatter: function (w) {
                                    return w.globals.seriesTotals.reduce((a, b) => {
                                        return a + b
                                    }, 0)
                                }
                            }
                        }
                    }
                }

            },

            series: result.series,
            labels: result.label,
            colors: ['#41A745', '#45A2B7','#DC3545']

        };
        var chart = new ApexCharts(document.querySelector("#status_request"), options);
        chart.render();

    }).fail((error) => {
        Swal.fire(
            'Opps!',
            'Sepertinya terjadi kesalahan Pada Chart status_request',
            'error'
        )
    })


    //stok
    $.ajax({
        url: "/users/GraphUser3",
        type: "GET",
    }).done((result) => {
        console.log(result);
        var optionss = {
            series: [{
                data: result.series
            }],
            chart: {
                type: 'bar',
                height: 200
            },
            plotOptions: {
                bar: {
                    borderRadius: 4,
                    horizontal: true,
                }
            },
            dataLabels: {
                enabled: false
            },
            xaxis: {
                categories: result.label,
            }
        };

        var charts = new ApexCharts(document.querySelector("#chart1"), optionss);
        charts.render();
    }).fail((result) => {

    });
    


});