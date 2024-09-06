export class PagedEnvelopDto<T> {
    items: T[] = [];
    totalCount: number = 0;
    pages: number = 0;
}