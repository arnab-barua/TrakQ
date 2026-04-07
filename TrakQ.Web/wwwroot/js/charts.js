(function () {
    'use strict';

    var defaultOptions = {
        responsive: true,
        maintainAspectRatio: true,
        plugins: {
            legend: { position: 'bottom' }
        }
    };

    function initChart(canvasId, config) {
        var el = document.getElementById(canvasId);
        if (!el) return;
        new Chart(el, config);
    }

    function initIncomeExpenseChart() {
        var data = window.__chartData && window.__chartData.incomeExpense;
        if (!data) return;
        initChart('incomeExpenseChart', {
            type: 'doughnut',
            data: data,
            options: defaultOptions
        });
    }

    function initTopHeadsChart() {
        var data = window.__chartData && window.__chartData.topHeads;
        if (!data) return;
        initChart('topHeadsChart', {
            type: 'bar',
            data: data,
            options: Object.assign({}, defaultOptions, {
                indexAxis: 'y',
                plugins: { legend: { display: false } },
                scales: { x: { beginAtZero: true } }
            })
        });
    }

    function initDailyChart() {
        var data = window.__chartData && window.__chartData.daily;
        if (!data) return;
        initChart('dailyChart', {
            type: 'bar',
            data: data,
            options: Object.assign({}, defaultOptions, {
                plugins: { legend: { display: false } },
                scales: { y: { beginAtZero: true } }
            })
        });
    }

    function initHeadShareChart() {
        var data = window.__chartData && window.__chartData.headShare;
        if (!data) return;
        initChart('headShareChart', {
            type: 'doughnut',
            data: data,
            options: defaultOptions
        });
    }

    function initIncomeChart() {
        var data = window.__chartData && window.__chartData.income;
        if (!data) return;
        initChart('incomeChart', {
            type: 'bar',
            data: data,
            options: Object.assign({}, defaultOptions, {
                indexAxis: 'y',
                plugins: { legend: { display: false } },
                scales: { x: { beginAtZero: true } }
            })
        });
    }

    function initTrendChart() {
        var data = window.__chartData && window.__chartData.trend;
        if (!data) return;
        initChart('trendChart', {
            type: 'line',
            data: data,
            options: Object.assign({}, defaultOptions, {
                plugins: { legend: { position: 'top' } },
                scales: { y: { beginAtZero: true } }
            })
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        initIncomeExpenseChart();
        initTopHeadsChart();
        initDailyChart();
        initHeadShareChart();
        initIncomeChart();
        initTrendChart();
    });
})();
