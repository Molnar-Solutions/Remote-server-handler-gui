import * as bcrypt from 'bcrypt';

export const salts = 12;

export const generatePasswordHash = async (password: string) => {
    const genSalts = bcrypt.genSaltSync(salts);
    return bcrypt.hashSync(password, genSalts);
};

export const isValidUserPassword = async (reqPassword: string, originalPassword: string) => {
    const isMatch = await bcrypt.compare(reqPassword, originalPassword);
    if (!isMatch) {
        console.warn("Failed to login, password not match.", { user: `System` });
        throw new Error("Belépés sikertelen, a megadott jelszavak nem egyeznek!");
    }

    return true;
}