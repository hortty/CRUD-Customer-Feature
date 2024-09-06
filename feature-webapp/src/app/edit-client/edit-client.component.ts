import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';
import { ClientService } from '../services/client.service';
import { CommonModule } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { MatNativeDateModule, MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { finalize, Observable, tap } from 'rxjs';
import { CustomerPostDto } from '../dtos/CustomerPostDto';
import { UpdateCustomerDto } from '../dtos/UpdateCustomerDto';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-edit-client',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    HttpClientModule,
    MatCheckboxModule,
    MatIconModule,
    NgxMaskDirective,
    MatOptionModule,
    MatSelectModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './edit-client.component.html',
  styleUrls: ['./edit-client.component.scss'],
  providers: [
    ClientService,
    provideNgxMask(),
  ],
})
export class EditClientComponent implements OnInit {
  clientForm: FormGroup;
  isEditMode: boolean = false;
  isLoading: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private clientService: ClientService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) {
    this.clientForm = this.fb.group(
      {
        name: ['', [Validators.required, Validators.maxLength(150)]],
        email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
        phone: ['', [Validators.required, Validators.maxLength(11), Validators.pattern('^[0-9]+$')]],
        personType: ['', Validators.required],
        cpfCnpj: [null],
        stateRegistration: ['', Validators.maxLength(12)],
        isExempt: [false],
        gender: [''],
        birthDate: [''],
        isBlocked: [false],
        passwordHash: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(15)]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validators: [this.passwordMatchValidator],
      }
    );

  }

  ngOnInit(): void {
    const clientId = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!clientId;

    if (this.isEditMode) {
      this.loadClientData(Number(clientId));
    }

    this.clientForm.get('isExempt')?.valueChanges.subscribe((hide) => {
      if (hide) {
        this.clientForm.get('stateRegistration')?.setValue('');
        this.clientForm.get('stateRegistration')?.disable();
      } else {
        this.clientForm.get('stateRegistration')?.enable();
      }
    });

    this.clientForm.get('personType')?.valueChanges.subscribe((type) => {
      if (type === 'Fisica') {
        this.clientForm.get('gender')?.setValidators([Validators.required]);
        this.clientForm.get('birthDate')?.setValidators([Validators.required]);
        this.clientForm.get('cpfCnpj')?.setValidators([Validators.required, Validators.pattern('\\d{11}')]);
        this.clientForm.get('cpfCnpj')?.updateValueAndValidity();
      } else {
        this.clientForm.get('cpfCnpj')?.setValidators([Validators.required, Validators.pattern('\\d{14}')]);
        this.clientForm.get('cpfCnpj')?.updateValueAndValidity();
        this.clientForm.get('gender')?.clearValidators();
        this.clientForm.get('birthDate')?.clearValidators();
        this.clientForm.get('gender')?.setValue('');
        this.clientForm.get('birthDate')?.setValue('');
      }
      this.clientForm.get('gender')?.updateValueAndValidity();
      this.clientForm.get('birthDate')?.updateValueAndValidity();
    });
  }

  loadClientData(clientId: number): void {
    this.isLoading = true;
    this.clientService.getClientById(clientId).subscribe({
      next: (client) => {
        this.clientForm.patchValue(client);
        this.clientForm.get('confirmPassword')?.setValue(client.passwordHash);
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        this.showSnackBar('Erro ao carregar dados do cliente.', 'Fechar', 'error-snackbar');
        console.error(err);
      },
    });
  }

  passwordMatchValidator(group: FormGroup): ValidationErrors | null {
    const password = group.get('passwordHash')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { mismatch: true };
  }

  hasError(controlName: string, errorName: string): boolean {
    const control = this.clientForm.get(controlName);

    if(control?.hasError(errorName) && (control.dirty || control.touched))
      return false;

    return true;
  }

  async saveClient(): Promise<void> {
    if (this.clientForm.invalid) {
      this.clientForm.markAllAsTouched();

      if(this.clientForm.get('passwordHash')?.valid && this.passwordMatchValidator(this.clientForm))
      {
        this.clientService.openErrorDialog('As senhas não conferem');
        return
      }
      else
      {
        this.showSnackBar('Por favor, preencha todos os campos obrigatórios corretamente.', 'Fechar', 'error-snackbar');
        return;
      }
    }

    try {
      
      this.isLoading = true;

      const clientData = this.clientForm.value;

      // aqui a senha poderia ser criptografada , e descriptografada no select, porém há limite 
      //de 15 caracteres então resolvi por não fazer isso já que também não foi um requisito

      if (clientData.personType === 'Fisica') {
        clientData.cpfCnpj = clientData.cpfCnpj.slice(0, 11);
      } else
      {
        clientData.gender = '';
        clientData.birthDate = '';
      }

      if(clientData.isExempt)
      {
        clientData.stateRegistration = '';
      }
      
      if (!clientData.isExempt &&
        (!clientData.stateRegistration || clientData.stateRegistration.length < 12)
      ) 
      {
        this.showSnackBar('Inscrição não pode estar vazia!', 'Fechar', 'error-snackbar');
        return;
      }
      

      if (this.isEditMode) {
        const clientId = Number(this.route.snapshot.paramMap.get('id'));

        if(clientId == 0)
          return;

        let updateCustomerDto: UpdateCustomerDto = new UpdateCustomerDto(
          clientId, clientData.name, clientData.email, clientData.phone,
          clientData.personType, clientData.cpfCnpj, clientData.stateRegistration,
          clientData.gender, clientData.isExempt, clientData.birthDate,
          clientData.isBlocked, clientData.passwordHash
        );

        this.clientService.updateClient(updateCustomerDto)
        .subscribe({
          next: () => {
            this.isLoading = false;
            this.showSnackBar('Cliente atualizado com sucesso!', 'Fechar', 'success-snackbar');
            this.router.navigate(['/clients']);
          },
          error: (err) => {
            this.isLoading = false;
      
            if (err.error && err.error.errors) {
              const firstErrorKey = Object.keys(err.error.errors)[0];
              const firstErrorMessage = err.error.errors[firstErrorKey][0];
      
              this.showSnackBar(firstErrorMessage, 'Fechar', 'error-snackbar');
            } else {
              if(err.error)
              {
                this.showSnackBar(`${err.error}`, 'Fechar', 'error-snackbar');
              } else
              {
                this.showSnackBar(`${err.message}`, 'Fechar', 'error-snackbar');
              }
            }
      
            console.error(err);
          }
        });
      } 
      else {

        let customerPostDto: CustomerPostDto = new CustomerPostDto(
          clientData.name, clientData.email, clientData.phone,
          clientData.personType, clientData.cpfCnpj, clientData.stateRegistration,
          clientData.gender, clientData.isExempt, clientData.birthDate,
          clientData.isBlocked, clientData.passwordHash
        );

        this.clientService.addClient(customerPostDto)
        .pipe(
          tap(() => console.log('Observable emitindo')),
          finalize(() => console.log('Observable finalizado'))
        )
        .subscribe({
          next: () => {
            console.log("boaa")
            this.isLoading = false;
            this.showSnackBar('Cliente adicionado com sucesso!', 'Fechar', 'success-snackbar');
            this.router.navigate(['/clients']);
          },
          error: (err) => {
            this.isLoading = false;
      
            if (err.error && err.error.errors) {
              const firstErrorKey = Object.keys(err.error.errors)[0];
              const firstErrorMessage = err.error.errors[firstErrorKey][0];
      
              this.showSnackBar(firstErrorMessage, 'Fechar', 'error-snackbar');
            } else {
              if(err.error)
              {
                this.showSnackBar(`${err.error}`, 'Fechar', 'error-snackbar');
              } else
              {
                this.showSnackBar(`${err.message}`, 'Fechar', 'error-snackbar');
              }
            }
      
            console.error(err);
          }
        });
      }
    } catch (error) {
      this.isLoading = false;
      this.showSnackBar('Ocorreu um erro inesperado.', 'Fechar', 'error-snackbar');
      console.error(error);
    }
  }

  showSnackBar(message: string, action: string, panelClass: string): void {
    this.snackBar.open(message, action, {
      duration: 5000,
      panelClass: [panelClass],
      horizontalPosition: 'right',
      verticalPosition: 'top',
    });
  }

  navigateBack(): void {
    this.router.navigate(['/clients']);
  }
}
