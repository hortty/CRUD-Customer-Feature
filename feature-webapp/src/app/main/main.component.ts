import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import { ClientService } from '../services/client.service';
import { FoundCustomerDto } from '../dtos/FoundCustomerDto';
import { PagedEnvelopDto } from '../dtos/PagedEnvelopDto';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { UpdateCustomerDto } from '../dtos/UpdateCustomerDto';

@Component({
  selector: 'app-main-content',
  standalone: true,
  imports: [
    MatButtonModule,
    MatCheckboxModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    FormsModule,
    CommonModule,
    MatIconModule,
    HttpClientModule,
    MatSnackBarModule
  ],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  providers: [
    ClientService
  ]
})
export class MainComponent implements OnInit {
  
  displayedColumns: string[] = ['select', 'name', 'email', 'phone', 'registrationDate', 'isBlocked', 'actions'];
  dataSource: FoundCustomerDto[] = [];
  selectedClients = new Set<number>();
  totalItems = 0;
  pageSize = 20;
  currentPage = 1;
  isLoading = false;

  constructor(private router: Router, private clientService: ClientService, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.loadClients();
  }

  loadClients(page: number = 1, pageSize: number = 20) {
    this.clientService.getPaginatedClients({ page, pageSize })
      .subscribe((response: PagedEnvelopDto<FoundCustomerDto>) => {
        this.dataSource = response.items;
        this.totalItems = response.totalCount;
        this.currentPage = page;
        this.pageSize = pageSize;
      });
  }

  editClient(clientId: number) {
    this.router.navigate(['/edit-client', clientId]);
  }

  addClient(): void {
    this.router.navigate(['/add-client']);
  }

  toggleAllClientsSelection(isChecked: boolean): void {
    if (isChecked) {
      this.selectedClients = new Set<number>(this.dataSource.map(client => client.id));
    } else {
      this.selectedClients.clear();
    }
  }

  toggleClientSelection(clientId: number, isChecked: boolean): void {
    if (isChecked) {
      this.selectedClients.add(clientId);
    } else {
      this.selectedClients.delete(clientId);
    }
  }

  isClientSelected(clientId: number): boolean {
    return this.selectedClients.has(clientId);
  }

  handlePageEvent(event: PageEvent) {
    this.loadClients(event.pageIndex + 1, event.pageSize);
  }

  showSearch(name: string) {
    if(name === undefined || name === null || name === '')
    {
      this.loadClients();
    }
    else
    {
      this.clientService.getPaginatedClientsByName({ name, page: 1, pageSize: 10 }).subscribe(response => {
        this.dataSource = response.items;
        this.totalItems = response.totalCount;
        this.currentPage = 1;
      });
    }
  }

  toggleBlock(clientId: number, block: boolean) {
    const clientData = this.dataSource.find(client => client.id === clientId);
  
    if (!clientData) {
      this.showSnackBar('Cliente não encontrado!', 'Fechar', 'error-snackbar');
      return;
    }

    if(clientData.gender == undefined || clientData.gender == null)
      clientData.gender = '';
  
    const updateCustomerDto: UpdateCustomerDto = new UpdateCustomerDto(
      clientId,
      clientData.name,
      clientData.email,
      clientData.phone,
      clientData.personType,
      clientData.cpfCnpj,
      clientData.stateRegistration,
      clientData.gender,
      clientData.isExempt,
      clientData.birthDate,
      block, 
      clientData.passwordHash
    );
  
    this.isLoading = true;
    this.clientService.updateClient(updateCustomerDto)
      .subscribe({
        next: () => {
          this.isLoading = false;
          this.showSnackBar('Estado de bloqueio atualizado com sucesso!', 'Fechar', 'success-snackbar');
          this.loadClients(this.currentPage, this.pageSize);
        },
        error: (err) => {
          this.isLoading = false;
  
          if (err.error && err.error.errors) {
            const firstErrorKey = Object.keys(err.error.errors)[0];
            const firstErrorMessage = err.error.errors[firstErrorKey][0];
  
            this.showSnackBar(firstErrorMessage, 'Fechar', 'error-snackbar');
          } else {
            if (err.error) {
              this.showSnackBar(`${err.error}`, 'Fechar', 'error-snackbar');
            } else {
              this.showSnackBar(`${err.message}`, 'Fechar', 'error-snackbar');
            }
          }
  
          console.error(err);
        }
      });
  }

  showSnackBar(message: string, action: string, panelClass: string): void {
    this.snackBar.open(message, action, {
      duration: 5000,
      panelClass: [panelClass],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }
}
