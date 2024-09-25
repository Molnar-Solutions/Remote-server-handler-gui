export function levenshteinDistance(sourceString: string, targetString: string): number {
    if (!sourceString || !targetString) {
        return sourceString ? sourceString.length : targetString.length;
    }

    const m = sourceString.length;
    const n = targetString.length;
    const distanceMatrix = Array.from({ length: m + 1 }, () => Array(n + 1).fill(0));

    for (let i = 0; i <= m; i++) {
        distanceMatrix[i][0] = i;
    }

    for (let j = 0; j <= n; j++) {
        distanceMatrix[0][j] = j;
    }

    for (let i = 1; i <= m; i++) {
        for (let j = 1; j <= n; j++) {
            const cost = sourceString[i - 1] === targetString[j - 1] ? 0 : 1;
            distanceMatrix[i][j] = Math.min(
                distanceMatrix[i - 1][j] + 1,
                distanceMatrix[i][j - 1] + 1,
                distanceMatrix[i - 1][j - 1] + cost
            );
        }
    }

    return distanceMatrix[m][n];

}