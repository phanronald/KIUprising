import * as fs from 'fs';
import * as path from 'path';
import * as process from 'process';
import * as child from 'child_process';

export class DirectoryService {

	
    public static IsFileExist = (filePath: string): void => {

        fs.exists(filePath, (exists) => {
            console.log('This exists: ' + exists);
        });
    }
    
    public static GetBaseName = (filePath: string):string => {
        return path.basename(filePath); 
    }

    public static GetCurrentDirectory = ():string => {
        return process.cwd(); 
    }

    public static GetDirectoryName = (filePath: string):string => {
        return path.dirname(filePath); 
    }

    public static GetChildWithoutStream = (shreddingPath:string, options: string[]): child.ChildProcessWithoutNullStreams => {

        return child.spawn(shreddingPath, options);
    }
	
}
