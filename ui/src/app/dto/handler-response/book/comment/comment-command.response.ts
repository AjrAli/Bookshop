import { CommentResponseDto } from "../../../book/comment/comment-response-dto";
import { CommandResponse } from "../../../response/command-response";

export class CommentCommandResponse extends CommandResponse {
    comment: CommentResponseDto;

    constructor(message?: string, error?: boolean, validationErrors?: string[]) {
        super(message, error, validationErrors);
        this.comment = new CommentResponseDto();
    }
}