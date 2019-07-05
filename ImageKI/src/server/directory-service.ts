import * as fs from 'fs';
import * as path from 'path';
import * as process from 'process';
import * as child from 'child_process';

export class DirectoryService {

    public static GetBaseName = (filePath: string):string => {
        return path.basename(filePath); 
    }

    public static GetCurrentDirectory = ():string => {
        return process.cwd(); 
    }

    public static GetDirectoryName = (filePath: string):string => {
        return path.dirname(filePath); 
    }

    public static GetChildWithoutStream = (exePath:string, options: string[]): child.ChildProcessWithoutNullStreams => {

        return child.spawn(exePath, options);
    }

    public static GetDirectoryFileContents = (directoryPath: string): void => {

        fs.readdir(directoryPath, (err, files) => {
            files.forEach(file => {
                if(!fs.statSync(DirectoryService.GetCurrentDirectory() + "\\" + file).isDirectory()) {
                    console.log(file);
                }
            });
        });
    }
    
    public static IsFileExist = (filePath: string): void => {

        fs.access(filePath, fs.constants.F_OK, (err) => {
            console.log(`${filePath} ${err ? 'does not exist' : 'exists'}`);
        });
    }

    public static IsFileReadonly = (filePath: string): void => {

        fs.access(filePath, fs.constants.R_OK, (err) => {
            console.log(`${filePath} ${err ? 'is not readable' : 'is readable'}`);
        });
    }

    public static IsFileWriteable = (filePath: string): void => {

        fs.access(filePath, fs.constants.W_OK, (err) => {
            console.log(`${filePath} ${err ? 'is not writable' : 'is writable'}`);
        });
    }

    public static IsFileExistsAndWriteable = (filePath: string): void => {

        fs.access(filePath, fs.constants.F_OK | fs.constants.W_OK, (err) => {
            if (err) {
              console.error(`${filePath} ${err.code === 'ENOENT' ? 'does not exist' : 'is read-only'}`);
            } else {
              console.log(`${filePath} exists, and it is writable`);
            }
        });
    }
}
