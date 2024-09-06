export class FoundCustomerDto {
    id!: number;
    name: string = '';
    cpfCnpj: string = '';
    email: string = '';
    phone: string = '';
    registrationDate!: Date;
    isBlocked!: boolean;
    personType: string = '';
    gender: string = '';
    stateRegistration: string = '';
    isExempt: boolean = false;
    birthDate?: Date = undefined;
    passwordHash: string = '';
}