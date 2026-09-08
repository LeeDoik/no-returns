shader_type spatial;
render_mode cull_back;
uniform sampler2D albedo_texture : source_color, filter_linear_mipmap_anisotropic;
uniform vec4 crew_color : source_color = vec4(0.74, 0.53, 0.25, 1.0);
void fragment() {
	vec3 base = texture(albedo_texture, UV).rgb;
	float hi = max(base.r, max(base.g, base.b));
	float lo = min(base.r, min(base.g, base.b));
	float span = max(hi - lo, 0.0001);
	float hue = hi == base.r ? (base.g-base.b)/span : (hi == base.g ? 2.0+(base.b-base.r)/span : 4.0+(base.r-base.g)/span);
	hue = fract(hue / 6.0 + 1.0);
	float saturation = span / max(hi, 0.0001);
	float mask = smoothstep(0.085,0.105,hue) * (1.0-smoothstep(0.18,0.205,hue)) * smoothstep(0.25,0.45,saturation);
	vec3 dyed = crew_color.rgb * clamp(dot(base, vec3(0.2126,0.7152,0.0722)) / 0.48, 0.25, 1.5);
	ALBEDO = mix(base, dyed, mask);
	ROUGHNESS = 0.82;
}
