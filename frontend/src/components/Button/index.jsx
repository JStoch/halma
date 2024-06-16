function Button({ value, className, href, target, onClick, disabled = false }) {
  return (
    <a
      href={href ? href : "javascript:;"}
      target={target ? target : "_self"}
      className={`flex justify-center items-center rounded px-4 py-2 text-neutral-800 ${className} ${
        disabled ? "bg-gray-500 hover:bg-gray-500 cursor-not-allowed" : ""
      }`}
      onClick={disabled ? () => {} : onClick}
    >
      <span className="font-bold">{value}</span>
    </a>
  );
}

export default Button;
