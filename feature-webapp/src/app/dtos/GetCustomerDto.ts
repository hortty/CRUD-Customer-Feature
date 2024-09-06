import { GetPagedBaseDto } from "./GetPagedBaseDto";

export class GetCustomerDto extends GetPagedBaseDto {
    id?: number;
    name?: string = '';
}