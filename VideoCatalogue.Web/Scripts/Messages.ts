/**
 * Application messages and user-facing text
 */

export const MESSAGES = {
  upload: {
    success: (fileCount: number) =>
      `Successfully uploaded ${fileCount} file(s).`,
    networkError: 'Network error. Please try again.',
    unexpectedError: 'An unexpected error occurred. Please try again.',
    defaultError: 'An error occurred during upload.',
  },
} as const
