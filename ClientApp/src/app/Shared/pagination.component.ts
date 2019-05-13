import { Component, Input, Output, EventEmitter } from '@angular/core';
import { OnChanges } from '@angular/core';

// tslint:disable-next-line:component-selector
@Component({
    selector: 'pagination',
    template: `
    <nav *ngIf="totalItems> pageSize">
        <ul class="pagination">
            <li [class.disabled]="currentPage == 1" class="page-item">
                <a (click)="previous()" aria-label="Previous" class="page-link">
                <span aria-hidden="true">&laquo;</span>
                </a>
            </li>
            <li [class.active]="currentPage == page" *ngFor="let page of pages" (click)="changePage(page)" class="page-item">
                <a class="page-link">{{ page }}</a>
            </li>
            <li [class.disabled]="currentPage == pages.length" class="page-item">
                <a (click)="next()" aria-label="Next" class="page-link">
                <span aria-hidden="true">&raquo;</span>
                </a>
            </li>
        </ul>
    </nav>
`
})
export class PaginationComponent implements OnChanges {
  // tslint:disable-next-line:no-input-rename
    @Input('total-items') totalItems;
    // tslint:disable-next-line:no-input-rename
    @Input('page-size') pageSize = 10;
    // tslint:disable-next-line:no-output-rename
    @Output('page-changed') pageChanged = new EventEmitter();
    pages: any[];
    currentPage = 1;

    ngOnChanges() {
        this.currentPage = 1;
        const pagesCount = Math.ceil(this.totalItems / this.pageSize);
        this.pages = [];
        for (let i = 1; i <= pagesCount; i++) {
        this.pages.push(i);
        }
    }

    changePage(page) {
        this.currentPage = page;
        this.pageChanged.emit(page);
    }

    previous() {
        if (this.currentPage === 1) {
            return;
        }

        this.currentPage--;
        this.pageChanged.emit(this.currentPage);
    }

    next() {
        if (this.currentPage === this.pages.length)
        {
            return;
        }

        this.currentPage++;
        this.pageChanged.emit(this.currentPage);
    }
}