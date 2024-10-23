import * as bcrypt from 'bcrypt';

/* If you prefer, increment / decrement it :) */
export const salts = 12;

export const generatePasswordHash = async (password: string) => {
  const genSalts = bcrypt.genSaltSync(salts);
  return bcrypt.hashSync(password, genSalts);
};

export const isValidUserPassword = async (
  reqPassword: string,
  originalPassword: string,
) => {
  const isMatch = await bcrypt.compare(reqPassword, originalPassword);
  if (!isMatch) {
    console.warn('Failed to login, password not match.', { user: `System` });
    throw new Error('Wowowo! Hey hey hey! You password is not match :(');
  }

  return true;
};
