import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ColumnDef, ColumnType } from '../../_models/columndef.model';

@Component({
  selector: 'datatable',
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.css']
})
export class DatatableComponent implements OnInit {

  @Input()
  set columns(columns: Array<ColumnDef>) {
    this.inputColumns = columns;
  }

  @Input()
  set data(data: Array<any>) {
    this.inputData = data;
    this.BindDataTable();
  }



  dataSource = new MatTableDataSource<any>([]);
  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator | undefined;
  @ViewChild(MatSort, { static: false }) sort: MatSort | undefined;
  displayColumns: Array<string> | undefined;
  filterColumns: Array<string> | undefined;
  inputColumns: Array<ColumnDef> | undefined;
  inputData: Array<any> | undefined;
  filter: any = {};

 

  @Output() rowclick = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }


  BindDataTable() {

    if (this.inputColumns == undefined) {
      if (this.inputData && this.inputData.length > 0) {
        this.displayColumns = Object.getOwnPropertyNames(this.inputData[0]);
        this.inputColumns = this.displayColumns.map(c => { return { name: c, displayName: c } });        
      }
    } else {
      this.displayColumns = this.inputColumns.map(c => c.name);
      this.inputColumns = this.inputColumns.map(c => {
        return { name: c.name, displayName: c.displayName ? c.displayName : c.name, width: c.width ? c.width : 'auto', columnType: c.columnType ? c.columnType : ColumnType.Text, class: c.class ? c.class : '', defaultText: c.defaultText, sort: c.sort }
      });    
    }

    this.filterColumns = this.displayColumns?.map(c => 'filter' + c);

    if (this.inputData && this.inputData.length > 0) {
      this.dataSource = new MatTableDataSource<any>(this.inputData);      
    } else {
      this.dataSource = new MatTableDataSource<any>([]);      
    }

      this.dataSource.paginator = this.paginator || null;
      this.dataSource.sort = this.sort || null;
      this.dataSource.filterPredicate = this.CreateFilter();

  }

  ButtonClick(element, columnName) {   
    let emmitValue= {
      name:columnName,
      value:element
    }
    this.rowclick.emit(emmitValue);
  }

  ShowFilter(index, item): boolean {
    return index == 0;
  }


  FilterData(event, column) {
    let value = event.srcElement.value;
    this.filter[column] = value;
    let json = JSON.stringify(this.filter);
    if (json == '{}') {
      this.dataSource.filter = undefined;
    } else {
      this.dataSource.filter = json;
    }
  }


  CreateFilter(): (data: any, filter: string) => boolean {
    return function (data, filter): boolean {
      let searchTerms = JSON.parse(filter);
      let columns = Object.keys(searchTerms);
      for (let index = 0; index < columns.length; index++) {
        let key = columns[index];
        if (data[key].toString().trim().indexOf(searchTerms[key].toString().trim()) !== -1) {
          if (index == columns.length - 1) {
            return true;
          }
        } else {
          return false;
        }
      }
    }
  }

}