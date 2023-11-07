namespace LLAMA_in_CSharp;

struct llama_model 
{
//     private EModel _type = EModel.ModelUnknown;
//     private LlmArch _arch = LlmArch.LlmArchUnknown;
//     private LlamaFtype _ftype = LlamaFtype.LlamaFtypeAllF32;
//
//     private string _name = "n/a";
//
//     private llama_hparams _hparams = new llama_hparams() {};
//     private llama_vocab _vocab;
//
//     partial struct ggml_tensor * tok_embeddings;
//
//     partial struct ggml_tensor * pos_embeddings;
//
//     partial struct ggml_tensor * tok_norm;
//
//     partial struct ggml_tensor * tok_norm_b;
//
//     partial struct ggml_tensor * output_norm;
//
//     partial struct ggml_tensor * output_norm_b;
//
//     partial struct ggml_tensor * output;
//
//     IList<llama_layer> layers;
//
//     int n_gpu_layers;
//
//     // context
//     struct ggml_context * ctx = NULL;
//
//     // the model memory buffer
//     llama_buffer buf;
//
//     // model memory mapped file
//     std::unique_ptr<llama_mmap> mapping;
//
//     // objects representing data potentially being locked in memory
//     llama_mlock mlock_buf;
//     llama_mlock mlock_mmap;
//
//     // for quantize-stats only
//     std::vector<std::pair<std::string,
//
//     partial struct ggml_tensor *>> tensors_by_name;
//
//     Int64 t_load_us = 0;
//     Int64 t_start_us = 0;
//
//     ~llama_model() {
//         if (ctx) {
//             ggml_free(ctx);
//         }
//
// #ifdef GGML_USE_CUBLAS
//         for (size_t i = 0; i < tensors_by_name.size(); ++i) {
//             ggml_cuda_free_data(tensors_by_name[i].second);
//         }
//         ggml_cuda_free_scratch();
// #elif defined(GGML_USE_CLBLAST)
//         for (size_t i = 0; i < tensors_by_name.size(); ++i) {
//             ggml_cl_free_data(tensors_by_name[i].second);
//         }
// #endif
//     }
// }
//
// struct llama_hparams 
// {
//     bool vocab_only;
//     Int32 n_vocab;
//     Int32 n_ctx_train; // context size the model was trained on
//     Int32 n_embd;
//     Int32 n_head;
//     Int32 n_head_kv;
//     Int32 n_layer;
//     Int32 n_rot;
//     Int32 n_ff;
//
//     float f_norm_eps;
//     float f_norm_rms_eps;
//
//     float rope_freq_base_train;
//     float rope_freq_scale_train;
//
//     float f_clamp_kqv;
//     float f_max_alibi_bias;
//
//     bool operator!=(const llama_hparams & other) const 
//     {
//         if (this->vocab_only != other.vocab_only) return true;
//         if (this->n_vocab != other.n_vocab) return true;
//         if (this->n_ctx_train != other.n_ctx_train) return true;
//         if (this->n_embd != other.n_embd) return true;
//         if (this->n_head != other.n_head) return true;
//         if (this->n_head_kv != other.n_head_kv) return true;
//         if (this->n_layer != other.n_layer) return true;
//         if (this->n_rot != other.n_rot) return true;
//         if (this->n_ff != other.n_ff) return true;
//
//         const float EPSILON = 1e-9;
//
//         if (!is_float_close(this->f_norm_eps, other.f_norm_eps, EPSILON)) return true;
//         if (!is_float_close(this->f_norm_rms_eps, other.f_norm_rms_eps, EPSILON)) return true;
//         if (!is_float_close(this->rope_freq_base_train, other.rope_freq_base_train, EPSILON)) return true;
//         if (!is_float_close(this->rope_freq_scale_train, other.rope_freq_scale_train, EPSILON)) return true;
//
//         return false;
//     }
//
//     Int32 n_gqa() const 
//     {
//         return n_head/n_head_kv;
//     }
//
//     Int32 n_embd_head() const 
//     {
//         return n_embd/n_head;
//     }
//
//     Int32 n_embd_gqa() const 
//     {
//         return n_embd/n_gqa();
//     }
// }
//
// struct llama_vocab 
// {
//     // I commented this out because I don't know how to translate it to C#
//     //using id    = int32_t;
//     //using token = std::string;
//     //using ttype = llama_token_type;
//
//     struct token_data 
//     {
//         token text;
//         float score;
//         ttype type;
//     }
//
//     enum llama_vocab_type type = LLAMA_VOCAB_TYPE_SPM;
//
//     std::unordered_map<token, id> token_to_id;
//     IList<token_data> id_to_token;
//
//     std::unordered_map<token, id> special_tokens_cache;
//
//     std::map<std::pair<string, string>, int> bpe_ranks;
//
//     // default LLaMA special tokens
//     id special_bos_id = 1;
//     id special_eos_id = 2;
//     id special_unk_id = 0;
//     id special_sep_id = -1;
//     id special_pad_id = -1;
//
//     id linefeed_id = 13;
//     id special_prefix_id = 32007;
//     id special_middle_id = 32009;
//     id special_suffix_id = 32008;
//     id special_eot_id = 32010;
//
//     int find_bpe_rank(string token_left, string token_right)
//     {
//         ExtensionMethods.replace_all(token_left,  " ",  "\u0120");
//         ExtensionMethods.replace_all(token_left,  "\n", "\u010A");
//         ExtensionMethods.replace_all(token_right, " ",  "\u0120");
//         ExtensionMethods.replace_all(token_right, "\n", "\u010A");
//
//         auto it = bpe_ranks.find(std::make_pair(token_left, token_right));
//         if (it == bpe_ranks.end()) 
//         {
//             return -1;
//         }
//
//         return it->second;
//     }
// }
//
// struct ggml_tensor {
//     enum ggml_type type;
//     enum ggml_backend_type backend;
//
//     struct ggml_backend_buffer * buffer;
//
//     int n_dims;
//     Int64 ne[GGML_MAX_DIMS]; // number of elements
//     Int64 nb[GGML_MAX_DIMS]; // stride in bytes:
//
//     // compute data
//     enum ggml_op op;
//
//     // op params - allocated as int32_t for alignment
//     Int32 op_params[GGML_MAX_OP_PARAMS / sizeof(int32_t)];
//
//     bool is_param;
//
//     struct ggml_tensor * grad;
//     struct ggml_tensor * src[GGML_MAX_SRC];
//
//     // performance
//     int perf_runs;
//     Int64 perf_cycles;
//     Int64 perf_time_us;
//
//     struct ggml_tensor * view_src;
//     size_t               view_offs;
//
//     void * data;
//
//     char name[GGML_MAX_NAME];
//
//     void * extra; // extra things e.g. for ggml-cuda.cu
//
//     char padding[12];
}