
import * as child from 'child_process';

export class ProcessService {

    public static GetChildWithoutStream = (exePath:string, options: string[]): child.ChildProcessWithoutNullStreams => {

        return child.spawn(exePath, options);
    }
}
