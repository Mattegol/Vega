import { Component, OnInit } from '@angular/core';


@Component({
    templateUrl: './admin.component.html',

})
export class AdminComponent implements OnInit {
    // data = {
    //     labels: ['BMW', 'Audi', 'Volvo'],
    //     datasets: [
    //         {
    //             data: [5, 3, 1],
    //             backgroundColor: [
    //                 '#ff6384',
    //                 '#36a2eb',
    //                 '#ffce56'
    //             ]
    //         }
    //     ]
    // };

    public pieChartLabels = ['Sales Q1', 'Sales Q2', 'Sales Q3', 'Sales Q4'];
    public pieChartData = [120, 150, 180, 90];
    public pieChartType = 'pie';

constructor() { }



ngOnInit() {
}

}
