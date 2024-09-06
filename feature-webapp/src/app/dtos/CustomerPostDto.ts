export class CustomerPostDto {
    constructor(
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

    name!: string;
    email!: string;
    phone!: string;
    personType!: string;
    cpfCnpj!: string;
    stateRegistration?: string;
    gender?: string;
    isExempt: boolean = false;
    birthDate?: Date;
    isBlocked!: boolean;
    passwordHash!: string;
}
