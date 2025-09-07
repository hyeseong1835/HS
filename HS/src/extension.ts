import * as vscode from 'vscode';
import fs from 'fs';
import { match } from 'assert';

// 부모 폴더 위치 얻기
function getParentDir(filePath: string): string | undefined {
    let lastSlashIndex = filePath.lastIndexOf('/');
    if (lastSlashIndex === -1)
    {
        lastSlashIndex = filePath.lastIndexOf('\\');

        if (lastSlashIndex === -1) { return undefined; }
    }

    return filePath.slice(0, lastSlashIndex);
}


async function pickWorkspaceAbsolutePath(): Promise<string | undefined> {
    const wss = vscode.workspace.workspaceFolders;
    if (!wss || wss.length === 0) {
        vscode.window.showErrorMessage('워크스페이스가 없습니다. 폴더를 먼저 여세요.');
        return undefined;
    }
    if (wss.length === 1) { return wss[0].uri.fsPath; }

    const items = wss.map(ws => ({
        label: ws.name,
        description: ws.uri.fsPath, // 절대 경로 미리 보여주기
        ws
    }));

    const sel = await vscode.window.showQuickPick(items, {
        placeHolder: '작업할 워크스페이스 폴더를 선택하세요',
        matchOnDescription: true
    });
    return sel?.ws.uri.fsPath; // 절대 경로 문자열 반환
}

async function pickSolutionAbsolutePath(
    workspaceDir: string
): Promise<string | undefined> {
    const slnUriList: vscode.Uri[] = await vscode.workspace.findFiles('**/*.sln', '**/node_modules/**', 100);
    if (slnUriList.length === 0) {
        return undefined;
    }

    if (slnUriList.length === 1) {
        return getParentDir(slnUriList[0].fsPath);
    }

    const items = slnUriList.map(uri => ({
        label: uri.fsPath.replace(workspaceDir, '').replace(/^[\\/]/, ''), // 워크스페이스 폴더 기준 상대 경로
        description: uri.fsPath,
    }));
    const sel = await vscode.window.showQuickPick(items, {
        placeHolder: '작업할 솔루션 파일을 선택하세요',
        matchOnDescription: true
    });
    return sel?.description? getParentDir(sel.description) : undefined; // 솔루션 파일의 디렉토리 경로 반환

}

// .git 디렉토리 경로 얻기
function getGitDir(
    startDir: string
): string | undefined {
    let curDir = startDir;

    while (true)
    {
        if (fs.existsSync(`${curDir}/.git`))
        {
            return curDir;
        }
        const parentDir = getParentDir(curDir);
        if (parentDir === undefined) {
            return undefined;
        }

        curDir = parentDir;
    }
}

// 터미널에서 명령 실행
async function executeInTerminal(
    cwd: string,
    cmd: string,
  ): Promise<number> {
    vscode.window.showInformationMessage(`명령 실행: ${cmd}`);
    const exec = new vscode.ShellExecution(cmd, { cwd: cwd });
    const task = new vscode.Task(
        { type: 'shell' },
        vscode.TaskScope.Workspace,
        'HS',
        'hs-ext',
        exec
    );
    task.presentationOptions = {
        reveal: vscode.TaskRevealKind.Always,
        panel: vscode.TaskPanelKind.Dedicated,
        clear: true
    };

    const endPromise = new Promise<number>((resolve) => {
        const d1 = vscode.tasks.onDidEndTaskProcess(e => {
            if (e.execution.task === task) {
                d1.dispose();
                resolve(e.exitCode ?? -1);
            }
        });
    });

    await vscode.tasks.executeTask(task);
    return await endPromise;
}

// 솔루션에 프로젝트 추가
async function addCsprojToSln(
    slnDir: string,
    slnPath: string,
    csprojPath: string
) {
    return executeInTerminal(
        slnDir,
        `dotnet sln ${slnPath} add ${csprojPath}`
    );
}

// HS 모듈 추가
// 반환: 모듈 디렉토리 경로
async function addHSModule(
    slnDir: string,
    repoName: string
):  Promise<string | undefined> {
    // .git 디렉토리 위치 얻기
    const gitDir: string | undefined = getGitDir(slnDir);
    if (gitDir === undefined)
    {
        vscode.window.showErrorMessage('솔루션 디렉토리의 상위에 .git 디렉토리가 없습니다.');
        return undefined;
    }

    // .gitmodules 파일이 없으면 생성
    const gitmodulesPath = `${gitDir}/.gitmodules`;
    if (false === fs.existsSync(gitmodulesPath))
    {
        const gitmodulesUri = vscode.Uri.file(gitmodulesPath);
        await vscode.workspace.fs.writeFile(gitmodulesUri, new Uint8Array());
    }

    const repoUrl = `https://github.com/hyeseong1835/${repoName}`;
    const moduleLocalDir = `SubModules/${repoName.split('.').join('/')}`;

    const code: number = await executeInTerminal(
        slnDir,
        `git submodule add \"${repoUrl}\" \"${moduleLocalDir}\"`
    );

    if (code !== 0)
    {
        vscode.window.showErrorMessage(`git submodule add 명령이 종료 코드 ${code}로 실패했습니다.`);
        return undefined;
    }

    return moduleLocalDir;
}

// 리포지토리 부모 이름 얻기
function getRepoParentName(
    repoName: string
): string | undefined {
    const lastPeriodIndex = repoName.lastIndexOf('.');
    if (lastPeriodIndex === -1) {
        return undefined;
    }
    return repoName.slice(0, lastPeriodIndex);
}

// 배열 합치기
function combineArray<T>(arr1: T[], arr2: T[]): T[] {
    let result: T[] = new Array<T>(arr1.length + arr2.length);
    for (let i = 0; i < arr1.length; i++)
    {
        result[i] = arr1[i];
    }
    for (let i = 0; i < arr2.length; i++)
    {
        result[i + arr1.length] = arr2[i];
    }
    return result;
}

// 가장 짧은 문자열의 인덱스 얻기
function getShortestStringIndex(arr: string[]) {
    if (arr.length === -1) {
        return -1;
    }
    let shortestStringIndex = 0;
    let shortestStringLength = arr[0].length;

    let curStringLength;
    for (let i = 1; i < arr.length; i++)
    {
        curStringLength = arr[i].length;
        if (curStringLength > shortestStringLength)
        {
            shortestStringIndex = i;
            shortestStringLength = curStringLength;
        }
    }
    return shortestStringIndex;
}

// 가장 짧은 문자열과 긴 문자열의 인덱스 얻기
function getShortestAndLongestStringIndex(
    arr: string[]): { shortest: number, longest: number }{
    // 길이가 0이면 {shortest: -1, longest: -1} 반환
    if (arr.length <= 0) {
        return {shortest: -1, longest: -1};
    }

    let shortestStringIndex = 0;
    let shortestStringLength = arr[0].length;

    let longestStringIndex = 0;
    let longestStringLength = arr[0].length;

    let curStringLength;
    for (let i = 1; i < arr.length; i++)
    {
        curStringLength = arr[i].length;
        if (curStringLength > shortestStringLength)
        {
            shortestStringIndex = i;
            shortestStringLength = curStringLength;
        }
        if (curStringLength < longestStringLength)
        {
            longestStringIndex = i;
            longestStringLength = curStringLength;
        }
    }
    return {shortest: shortestStringIndex, longest: longestStringIndex};
}

// 문자열 보유 여부 얻기
function hasString(arr: string[], str: string): boolean {
    for (const s of arr)
    {
        if (s === str) {
            return true;
        }
    }
    return false;
}

// QuickPick 새로고침
function refreshQuickPick(
    quickPick: vscode.QuickPick<vscode.QuickPickItem>,
    repoNameList: string[]
) {
    const curRepoParentName: string | undefined = getRepoParentName(quickPick.value);
    const nearRepoNameList: string[] = (curRepoParentName === undefined)? repoNameList : repoNameList.filter(repo => repo.startsWith(curRepoParentName));

    const matchedRepoNameList: string[] = nearRepoNameList.filter(name => name.startsWith(quickPick.value));
    const notMatchedRepoNameList: string[] = nearRepoNameList.filter(name => false === name.startsWith(quickPick.value));

    // 일치하는 리포지토리 이름을 앞으로 이동
    const sortedRepoList = combineArray(matchedRepoNameList, notMatchedRepoNameList);

    // 가장 짧은 일치하는 리포지토리 이름의 인덱스
    const shortestMatchedRepoNameIndex = getShortestStringIndex(matchedRepoNameList);

    quickPick.items = sortedRepoList.map<vscode.QuickPickItem>(
        (value, index, array) => ({
            label: value,
            picked: (index === shortestMatchedRepoNameIndex)
        })
    );
    quickPick.placeholder = matchedRepoNameList[shortestMatchedRepoNameIndex];
    quickPick.activeItems = matchedRepoNameList.map<vscode.QuickPickItem>(
        (value, index, array) => ({
            label: value,
            picked: (index === shortestMatchedRepoNameIndex)
        })
    );
}

// 리포지토리 입력
async function inputRepoName(
    repoNameList: string[]
): Promise<string | undefined> {

    const quickPick = vscode.window.createQuickPick();
    quickPick.title = '리포지토리 이름';
    quickPick.value = 'HS.';
    refreshQuickPick(quickPick, repoNameList);

    quickPick.onDidAccept(function() {
        const matchedRepoNameList: string[] = repoNameList.filter(name => name.startsWith(quickPick.value));
        if (matchedRepoNameList.length === 0)
        {
            vscode.window.showErrorMessage("일치하는 리포지토리를 찾을 수 없습니다.");
            return undefined;
        }

        const longestAndShortestIndex = getShortestAndLongestStringIndex(matchedRepoNameList);

        // 일치하는 모듈 존재 => 닫기 (반환)
        if (quickPick.value.length === matchedRepoNameList[longestAndShortestIndex.longest].length)
        {
            quickPick.hide();
            return undefined;
        }

        // 가장 가까운 일치하는 경로로 자동완성
        const shortestRepoName = matchedRepoNameList[longestAndShortestIndex.shortest];
        const nextPointIndex = shortestRepoName.indexOf('.', quickPick.value.length);
        quickPick.value = (nextPointIndex === -1)
                          ? shortestRepoName
                          : shortestRepoName.slice(0, nextPointIndex);

        refreshQuickPick(quickPick, repoNameList);
        return undefined;
    });
    quickPick.onDidChangeValue(function() {
        refreshQuickPick(quickPick, repoNameList);
    });

    quickPick.show();

    // 사용자가 닫을 때 최종 값 반환 (취소 시 빈 문자열일 수 있음)
    return await new Promise<string | undefined>(resolve => {
        const sub = quickPick.onDidHide(() => {
            sub.dispose();
            resolve(quickPick.value || undefined);
            quickPick.dispose();
        });
    });
}

//----------------------------------------------------------------------------
// 모듈 참조 명령
//----------------------------------------------------------------------------
async function referenceModule() {

    const out: vscode.OutputChannel = vscode.window.createOutputChannel('HS');
    out.show(true);

    // 작업 영역
    const workspaceFolder = await pickWorkspaceAbsolutePath();
    if (workspaceFolder === undefined) {
        vscode.window.showErrorMessage('워크스페이스가 없습니다.');
        return undefined;
    }
    out.appendLine(`워크스페이스: ${workspaceFolder}`);

    // 솔루션 디렉토리
    const slnPath = await pickSolutionAbsolutePath(workspaceFolder);
    if (slnPath === undefined) {
        vscode.window.showErrorMessage('솔루션 파일이 없습니다.');
        return undefined;
    }
    const slnDir = getParentDir(slnPath);
    if (slnDir === undefined) {
        vscode.window.showErrorMessage('솔루션 디렉토리 경로를 분석할 수 없습니다.');
        return undefined;
    }
    out.appendLine(`솔루션 위치: ${slnPath}`);

    // config
    const config = vscode.workspace.getConfiguration('hs');

    // 리포지토리 목록
    const repos = config.get<string[]>('repoDefs');
    if (repos === undefined || repos.length === 0)
    {
        vscode.window.showErrorMessage('리포지토리 목록이 비어있습니다.');
        return undefined;
    }
    out.appendLine(`리포지토리 목록: ${repos.join(', ')}`);

    // 타깃 리포지토리 이름 입력
    const repoName = await inputRepoName(repos);
    if (repoName === undefined) { return; }
    if (false === hasString(repos, repoName)) { return; }
    out.appendLine(`선택된 리포지토리: ${repoName}`);

    // 리포지토리 모듈 참조
    const repoProjDir = await addHSModule(
        slnPath,
        repoName
    );
    if (repoProjDir === undefined) { return; }
    out.appendLine(`참조된 모듈의 저장 위치: ${repoProjDir}`);

    addCsprojToSln(
        slnDir,
        slnPath,
        `${repoProjDir}/${repoName}.csproj`
    );
    out.appendLine(`솔루션에 프로젝트 추가됨: ${repoProjDir}/${repoName}.csproj`);
    return;
}

//----------------------------------------------------------------------------
// 확장 기능 활성화 이벤트
//----------------------------------------------------------------------------
export function activate(context: vscode.ExtensionContext) {
    context.subscriptions.push(
        vscode.commands.registerCommand('hs.referenceModule', referenceModule)
    );
    vscode.window.showInformationMessage('HS 확장 기능이 활성화되었습니다.');
}

//----------------------------------------------------------------------------
// 확장 기능 비활성화 이벤트
//----------------------------------------------------------------------------
export function deactivate() {}
