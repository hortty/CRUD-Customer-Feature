export class UpdateCustomerDto {

    constructor(
        id: number,
        name: string = '',
        email: string = '',
        phone: string = '',
        personType: string = '',
        cpfCnpj: string = '',
        stateRegistration?: string,
        gender?: string,
        isExempt: boolean = false,
        birthDate: Date = new Date('0001-01-01T00:00:00Z'),
        isBlocked: boolean = false,
        passwordHash: string = ''
    ) 
    {
        this.id = id,
        this.name = name;
        this.email = email;
        this.phone = phone;
        this.personType = personType;
        this.cpfCnpj = cpfCnpj;
        this.stateRegistration = stateRegistration;
        this.gender = gender;
        this.isExempt = isExempt;
        this.birthDate = birthDate;
        this.isBlocked = isBlocked;
        this.passwordHash = passwordHash;
    }

    id!: number;
    name!: string;
    email!: string;
    phone!: string;
    passwordHash!: string;
    personType!: string;
    cpfCnpj!: string;
    isExempt: boolean = false;
    stateRegistration?: string;
    gender?: string;
    birthDate?: Date;
    isBlocked!: boolean;
}