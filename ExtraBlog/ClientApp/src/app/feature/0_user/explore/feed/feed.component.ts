import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BehaviorSubject } from 'rxjs';
import { take, tap } from 'rxjs/operators';
import { AlertifyService } from 'src/app/core/services/alertify.service';
import { DataService } from 'src/app/core/services/data.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent {
  numberOfEntities: number = 0;
  documentsNewsFeed$ = new BehaviorSubject<any[]>([]);
  JSON = JSON

  myDocuments$ = new BehaviorSubject<any>(this.data.getMyDocuments());
  allCategories$ = new BehaviorSubject<any>(this.data.getAllCategories());

  constructor(private data: DataService, public dialog: MatDialog, private alertify: AlertifyService) { }

  selectedIndexChange(event) {
    if(event === 0) {
      this.documentsNewsFeed$.next([])
      this.numberOfEntities = 0;
      this.onScroll();
    }
  }

  openDialog(category, edit = false, document = {}): void {
    const dialogRef = this.dialog.open(DialogOverviewExampleDialog, {
      width: category ? '400px' : '1000px',
      height: category ? '200px' : '600px',
      data: {
        category: category,
        edit: edit,
        ...document
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if(!result)
        return
      let ob;
      if(category) {
        ob = this.data.saveCategory(result)
      } else if(!result.edit){
        ob = this.data.saveDocument(result)
      } else 
        ob = this.data.editDocument(result)
      ob.subscribe(res => {
        this.myDocuments$.next(this.data.getMyDocuments());
        this.allCategories$.next(this.data.getAllCategories());
      });
    });
  }

  openDocumentPreview(document) {
    const dialogRef = this.dialog.open(DocumentPreview, {
      width: '1000px',
      data: {
        document: document
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
      console.log(result)
    });
  }

  onScroll() {
    this.data.getNewsFeed(this.numberOfEntities)
      .pipe(
        tap((res) => { this.numberOfEntities += res.length }), 
        take(1)
      )
      .subscribe(res => {
        if(res)
          this.documentsNewsFeed$.next([...this.documentsNewsFeed$.value, ...res])
      })
  }

  subscribeToCategory(categoryName) {
    this.data.followCategory(categoryName).subscribe(res=>this.alertify.success('Uspesno ste se pretplatili!'));
  }

  delete(name) {
    this.data.deleteDocument(name).subscribe(res => {
      this.myDocuments$.next(this.data.getMyDocuments());
      this.allCategories$.next(this.data.getAllCategories());
    })
  }

}

@Component({
  templateUrl: 'dialog-overview-example-dialog.html'
})
export class DialogOverviewExampleDialog {
  categories$ = this.dataService.getAllCategories();
  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService) {
      if(data.pictures) {
        data.pictures = data.pictures.join(';');
      }
    }

  onNoClick(): void {
    this.dialogRef.close();
  }

}

@Component({
  templateUrl: 'document.preview.html'
})
export class DocumentPreview implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<DialogOverviewExampleDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any, public dataService: DataService) {

    }

  ngOnInit(): void {
    this.dataService.seen(this.data.document.name).subscribe();
  }
  
  onNoClick(): void {
    this.dialogRef.close();
  }

}

