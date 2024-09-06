import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable, of, throwError } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
// import { CustomerPostDto, CreatedCustomerDto, FoundCustomerDto, GetCustomerDto, UpdateCustomerDto, UpdatedCustomerDto, DeleteCustomerDto, DeletedCustomerDto, PagedEnvelopDto };
import { ErrorDialogComponent } from '../error-dialog/error-dialog.component';
import { FoundCustomerDto } from '../dtos/FoundCustomerDto';
import { PagedEnvelopDto } from '../dtos/PagedEnvelopDto';
import { CreatedCustomerDto } from '../dtos/CreatedCustomerDto';
import { CustomerPostDto } from '../dtos/CustomerPostDto';
import { UpdateCustomerDto } from '../dtos/UpdateCustomerDto';
import { UpdatedCustomerDto } from '../dtos/UpdatedCustomerDto';
import { DeletedCustomerDto } from '../dtos/DeletedCustomerDto';
import { GetCustomerDto } from '../dtos/GetCustomerDto';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
  // imports: [
  //   MatSnackBarModule,
  // ]
})
export class ClientService {
  private apiUrl = '';

  constructor(private http: HttpClient, private dialog: MatDialog, private snackBar: MatSnackBar) 
  {
    this.apiUrl = "http://localhost:5000/Customer";
  }

  getClientById(id: number): Observable<FoundCustomerDto> {
    return this.http.get<FoundCustomerDto>(`${this.apiUrl}/${id}`);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  getPaginatedClients(getCustomerDto: GetCustomerDto): Observable<PagedEnvelopDto<FoundCustomerDto>> {
    return this.http.get<PagedEnvelopDto<FoundCustomerDto>>(`${this.apiUrl}/paginated/${getCustomerDto.page}/${getCustomerDto.pageSize}`);
  }

  getPaginatedClientsByName(getCustomerDto: GetCustomerDto): Observable<PagedEnvelopDto<FoundCustomerDto>> {
    return this.http.get<PagedEnvelopDto<FoundCustomerDto>>(`${this.apiUrl}/paginated-by-name/${getCustomerDto.name}/${getCustomerDto.page}/${getCustomerDto.pageSize}`);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  addClient(client: CustomerPostDto): Observable<CreatedCustomerDto> {
    client.birthDate = this.adjustDateValue(client.birthDate);
    return this.http.post<CreatedCustomerDto>(this.apiUrl, client);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  updateClient(client: UpdateCustomerDto): Observable<UpdatedCustomerDto> {
    client.birthDate = this.adjustDateValue(client.birthDate);
    return this.http.put<UpdatedCustomerDto>(`${this.apiUrl}/${client.id}`, client);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  deleteClient(id: number): Observable<DeletedCustomerDto> {
    return this.http.delete<DeletedCustomerDto>(`${this.apiUrl}/${id}`);
    // .pipe(
    //   catchError(this.handleError)
    // );
  }

  private handleError = (error: HttpErrorResponse): Observable<never> => {
    let errorMessage: string = 'Erro desconhecido ocorreu!';
  
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
  
    this.openErrorDialog(errorMessage);
  
    return throwError(() => new Error(errorMessage));
  };

  // adjustDateValue(dateValue: Date | null | undefined): Date {
  //   const MIN_DATE = new Date('0001-01-01T00:00:00Z');
  
  //   if (dateValue === null || dateValue === undefined) {
  //     return MIN_DATE;
  //   }
  
  //   return dateValue;
  // }

  adjustDateValue(dateValue: Date | string | null | undefined): Date | undefined {
    const MIN_DATE = new Date('0001-01-01T00:00:00Z');
  
    if (dateValue === '' || dateValue === null || dateValue === undefined) {
      return undefined;
    }
  
    if (typeof dateValue === 'string') {
      const parsedDate = new Date(dateValue);
      return isNaN(parsedDate.getTime()) ? MIN_DATE : parsedDate;
    }
  
    return isNaN((dateValue as Date).getTime()) ? MIN_DATE : (dateValue as Date);
  }
  
  
  public openErrorDialog(message: string): void {
    this.snackBar.open(message, 'Fechar', {
      duration: 3000,
      verticalPosition: 'top',
    });

    // this.dialog.open(ErrorDialogComponent, {
    //   data: { message },
    //   width: '300px'
    // });
  }
}
