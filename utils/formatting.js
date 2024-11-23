export const formatNumber = (number, locale) => {
  return new Intl.NumberFormat(locale).format(number)
}

export const formatDate = (date, locale) => {
  return new Intl.DateTimeFormat(locale).format(date)
} 