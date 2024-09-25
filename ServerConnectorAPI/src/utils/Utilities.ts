import slugify from 'slugify';

export const convertToSlug = (title: string) => {
  const options: any = {
    lower: true,
    strict: true,
  };

  return slugify(title, options);
}