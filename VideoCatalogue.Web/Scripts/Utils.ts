import { MESSAGES } from './Messages.js'

/**
 * Utility functions for the application
 */

/**
 * Error response format from ASP.NET Core ValidationProblemDetails
 */
export interface IErrorResponse {
  type?: string
  title?: string
  status?: number
  errors?: { [key: string]: string[] }
  traceId?: string
}

/**
 * Converts bytes to a human-readable format (KB/MB/GB)
 * @param bytes - The number of bytes to convert
 * @returns A formatted string (e.g., "1.5 MB")
 */
export function formatBytes(bytes: number): string {
  if (bytes === 0) return '0 B'

  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB', 'TB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))

  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i]
}

/**
 * Extracts and concatenates error messages from ASP.NET Core ValidationProblemDetails format
 * @param errorData - The error response object from ASP.NET Core
 * @returns A string containing all error messages joined by newlines
 */
export function extractErrorMessages(errorData: IErrorResponse): string {
  if (!errorData.errors) {
    return errorData.title || MESSAGES.upload.defaultError
  }

  // Extract all error messages from the errors object
  const allErrors: string[] = []
  Object.keys(errorData.errors).forEach((key) => {
    const errorMessages = errorData.errors![key]
    if (Array.isArray(errorMessages)) {
      allErrors.push(...errorMessages)
    }
  })

  // Join all error messages with newlines
  return allErrors.length > 0
    ? allErrors.join('\n')
    : errorData.title || MESSAGES.upload.defaultError
}
