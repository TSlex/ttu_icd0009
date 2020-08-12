import { IFetchResponse } from 'types/Response/IFetchResponseDTO';

export async function parseResponse<TData>(response: Response): Promise<IFetchResponse<TData>> {
    let obj: IFetchResponse<TData>;

    try {
        obj = (await response.json()) as IFetchResponse<TData>;
    } catch (e) {
        obj = {} as IFetchResponse<TData>;
    }

    if ('title' in obj && (obj as any)['title'].indexOf('validation') !== -1) {

        let errors = (obj as any).errors as ModelStateErrors
        let errorsArray: string[] = []

        Object.values(errors).forEach((item: string[]) => {
            item.forEach((error: string) => {
                errorsArray.push(error);
            });
        });

        (obj as IFetchResponse<TData>).errors = errorsArray
    }

    return obj;
}

interface ModelStateErrors {
    [propName: string]: string[]
}
