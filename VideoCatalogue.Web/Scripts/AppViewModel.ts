import { formatBytes, extractErrorMessages, IErrorResponse } from './Utils.js'
import { MESSAGES } from './Messages.js'
import { API_ENDPOINTS } from './Constants.js'

/**
 * Response model from the API
 */
interface IVideoItem {
  url: string
  fileName: string
  sizeInBytes: number
}

/**
 * View model for displaying video item in the UI
 */
interface IVideoItemViewModel extends IVideoItem {
  formattedSize: string
}

class ViewModel {
  public isCatalogueView = ko.observable<boolean>(true)
  public isUploadFormView = ko.observable<boolean>(false)

  // Stores the list of all video files
  public videoCatalogue = ko.observableArray<IVideoItemViewModel>([])

  // === Video Playback Control ===
  // Currently playing video URL
  public currentVideoUrl = ko.observable<string>('')
  // Currently playing video name
  public currentVideoName = ko.observable<string>('')

  // === File Upload Data ===
  // Stores the list of files selected by the user
  public filesToUpload = ko.observableArray<File>([])
  public isUploading = ko.observable<boolean>(false)
  public uploadErrorMessage = ko.observable<string>('')
  public uploadSuccessMessage = ko.observable<string>('')

  constructor() {
    // Computed properties to ensure views are mutually exclusive
    this.isUploadFormView.subscribe((newValue) => {
      this.isCatalogueView(!newValue)
    })
    this.isCatalogueView.subscribe((newValue) => {
      this.isUploadFormView(!newValue)
    })
  }

  public showCatalogue = () => {
    this.resetMessages()
    this.isCatalogueView(true)
    this.loadCatalogue()
  }

  public showUploadForm = () => {
    this.resetMessages()
    this.isUploadFormView(true)
  }

  private resetMessages = () => {
    this.uploadErrorMessage('')
    this.uploadSuccessMessage('')
  }

  public async loadCatalogue() {
    try {
      const response = await axios.get<IVideoItem[]>(API_ENDPOINTS.catalogue)
      const data = response.data
      const formattedData = data.map((item) => ({
        ...item,
        formattedSize: formatBytes(item.sizeInBytes),
      }))
      this.videoCatalogue(formattedData)
    } catch (error: any) {
      console.error('Error loading catalogue:', error)
    }
  }

  public selectVideo = (item: IVideoItemViewModel) => {
    if (this.currentVideoName() === item.fileName) {
      return
    }

    this.currentVideoUrl(item.url)
    this.currentVideoName(item.fileName)
  }

  public onFilesSelected = (vm: ViewModel, event: Event) => {
    const input = event.target as HTMLInputElement
    this.filesToUpload.removeAll()
    if (input.files) {
      Array.from(input.files).forEach((file) => {
        this.filesToUpload.push(file)
      })
    }
  }

  public submitFiles = async () => {
    if (this.filesToUpload().length === 0) return

    this.isUploading(true)
    this.resetMessages()

    const formData = new FormData()
    this.filesToUpload().forEach((file) => {
      formData.append('files', file)
    })

    try {
      await axios.post(API_ENDPOINTS.upload, formData)

      this.uploadSuccessMessage(
        MESSAGES.upload.success(this.filesToUpload().length)
      )
      this.filesToUpload.removeAll()
      this.showCatalogue()
    } catch (error: any) {
      if (error.response && error.response.data) {
        const errorData: IErrorResponse = error.response.data
        const errorMessages = extractErrorMessages(errorData)
        this.uploadErrorMessage(errorMessages)
      } else if (error.request) {
        this.uploadErrorMessage(MESSAGES.upload.networkError)
      } else {
        this.uploadErrorMessage(MESSAGES.upload.unexpectedError)
      }
    } finally {
      this.isUploading(false)
    }
  }
}

export { ViewModel }
